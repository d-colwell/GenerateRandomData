using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenerateRandomData
{
    public class Example
    {
        [Key]
        public int Index { get; set; }

        public DateTime Date { get; set; }

        public Guid UniqueID { get; set; }
        [MinLength(8)]
        [MaxLength(16)]
        public string Text { get; set; }

        [Range(-200,10000)]
        public decimal RandomDecimal { get; set; }

        [Range(0,1000000)]
        public int RandomInt { get; set; }
    }
}
