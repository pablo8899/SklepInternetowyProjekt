using SklepInternetowy.Authentication;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SklepInternetowy.Entities
{
    public class ShoppingCartEntity
    {
        public int Id { get; set; }
        public virtual ShippingEntity Shipping { get; set; }
        public virtual PromoCodeEntity PromoCode { get; set; }

    }
}
