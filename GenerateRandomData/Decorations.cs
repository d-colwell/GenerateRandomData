using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenerateRandomData
{
    public class DataDefinition
    {
        public Type Type { get; set; }
        public bool IsKey { get; set; }
        public int? MinLength  { get; set; }
        public int? MaxLength { get; set; }
        public double? MinValue { get; set; }
        public double? MaxValue { get; set; }
    }
}
