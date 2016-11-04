using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    public class SalesOrder

    {
        [Range(71774,int.MaxValue)]
        public int SalesOrderID { get; set; }
        [Key]   
        public int SalesOrderDetailID { get; set; }
        [Range(1,10)]
        public int OrderQuantity { get; set; }

        [Range(500,999)]
        public int ProductID { get; set; }
        [Range(100,10000)]
        public double UnitPrice { get; set; }
        [Range(0,1)]
        public double UnitPriceDiscount { get; set; }

        public double LineTotal { get { return (UnitPrice * OrderQuantity) * (1 - UnitPriceDiscount); } }
        public Guid rowguid { get; set; }
        public DateTime ModifiedDate { get { return DateTime.Today; } }
    }
}
