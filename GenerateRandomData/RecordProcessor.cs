using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
namespace GenerateRandomData
{
    public class RecordProcessor<T>
        where T: class, new() 
    {
        private static Random random = new Random();
        public int KeyValue { get; set; }
        ///Considerations
        /// Primary Key (AutoInc => [Key]
        /// Min/Max (use DataAnnotations)
        /// Type -> Implicit
        /// Random (new annotation?)

        public IEnumerable<T> CreateRecords(int recordCount)
        {
            Type typeDefinition = typeof(T);
            var properties = typeDefinition.GetProperties().Where(p=>p.SetMethod != null && p.SetMethod.IsPublic);
            IList<KeyValuePair<PropertyInfo, DataDefinition>> decoratedProperties = new List<KeyValuePair<PropertyInfo, DataDefinition>>();
            foreach (var property in properties)
            {
                var decorations = GetDataDefinition(property);
                decoratedProperties.Add(new KeyValuePair<PropertyInfo, DataDefinition>(property, decorations));
            }

            for (int i = 0; i < recordCount; i++)
            {
                T record = Activator.CreateInstance<T>();
                foreach (var property in decoratedProperties)
                {
                    object propertyValue = GetValue(property.Value);
                    property.Key.SetValue(record, propertyValue);
                }
                yield return record;
            }
        }

        private DataDefinition GetDataDefinition(PropertyInfo property)
        {
            var decorations = new DataDefinition();
            var keyAttribute = property.GetCustomAttribute<KeyAttribute>(true);
            var rangeAttribute = property.GetCustomAttribute<RangeAttribute>(true);
            var minLengthAttribute = property.GetCustomAttribute<MinLengthAttribute>(true);
            var maxLengthAttribute = property.GetCustomAttribute<MaxLengthAttribute>(true);
            decorations.Type = property.PropertyType;
            if (keyAttribute != null)
                decorations.IsKey = true;
            if(rangeAttribute != null)
            {
                decorations.MinValue = double.Parse(rangeAttribute.Minimum.ToString());
                decorations.MaxValue = double.Parse(rangeAttribute.Maximum.ToString());
            }
            if(minLengthAttribute != null)
                decorations.MinLength = minLengthAttribute.Length;
            if (maxLengthAttribute != null)
                decorations.MaxLength = maxLengthAttribute.Length;
            return decorations;
        }
        private object GetValue(DataDefinition definition)
        {
            if (definition.IsKey)
                return KeyValue++;

            if (definition.Type == typeof(int))
                return (int)GetNumeric(definition);

            if(definition.Type == typeof(DateTime))
                return GetDate(definition);

            if (definition.Type == typeof(string))
                return GetString(definition);
            if (definition.Type == typeof(Guid))
                return GetGuid(definition);
            //Numeric catch all
            return Convert.ChangeType(GetNumeric(definition), definition.Type);
        }

        private DateTime GetDate(DataDefinition definition)
        {
            return DateTime.MinValue.AddDays(random.Next(10, 1000000));
        }

        private Guid GetGuid(DataDefinition definition)
        {
            return Guid.NewGuid();
        }

        private string GetString(DataDefinition definition)
        {
            var totalCharacters = random.Next(definition.MinLength.Value, definition.MaxLength.Value);
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, totalCharacters)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        private double GetNumeric(DataDefinition definition)
        {
            double minVal = int.MinValue;
            double maxVal = int.MaxValue;
            if (definition.MinValue.HasValue)
                minVal = Math.Max(definition.MinValue.Value, int.MinValue);
            if (definition.MaxValue.HasValue)
                maxVal = Math.Min(definition.MaxValue.Value, int.MaxValue);
            int generatedNumber = random.Next((int)minVal, (int)maxVal);
            int randomPrecision = random.Next(0, 100);
            return generatedNumber + (randomPrecision / 100.00);
        }
    }
}
