using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SklepInternetowy.Entities
{
    public class ShippingEntity
    {
        public int Id { get; set; }
        public string Delivery { get; set; }
        public double Price { get; set; }
    }
}
