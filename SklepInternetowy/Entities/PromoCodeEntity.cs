using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SklepInternetowy.Entities
{
    public class PromoCodeEntity
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public int DiscountPercent { get; set; }
    }
}
