using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Product
    {
        public string Name { get; set; }
        public double QuantitySold { get; set; }
        public double PricePerUnit { get; set; }
        public double TotalPrice { get; set; }
    }
}
