using GenerateRandomData;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            var rp = new RecordProcessor<SalesOrder>();
            rp.KeyValue = 30001;
            var records = rp.CreateRecords(10000000);
            using (var streamWriter = new StreamWriter(@"C:\Temp\ihopethisworks.csv",false))
            {
                ToCsv<SalesOrder>("|", records,streamWriter);
            }
        }

        public static void ToCsv<T>(string separator, IEnumerable<T> objectlist, StreamWriter stream)
        {
            Type t = typeof(T);
            PropertyInfo[] fields = t.GetProperties();

            string header = String.Join(separator, fields.Select(f => f.Name).ToArray());

            stream.WriteLine(header);

            foreach (var o in objectlist)
                stream.WriteLine(ToCsvFields(separator, fields, o));

        }

        public static string ToCsvFields(string separator, PropertyInfo[] fields, object o)
        {
            StringBuilder linie = new StringBuilder();

            foreach (var f in fields)
            {
                if (linie.Length > 0)
                    linie.Append(separator);

                var x = f.GetValue(o);

                if (x != null)
                    linie.Append(x.ToString());
            }

            return linie.ToString();
        }
    }
}
