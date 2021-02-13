using SklepInternetowy.Authentication;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SklepInternetowy.Entities
{
    public class ShoppingCartItemEntity
    {
        public int Id { get; set; }
        public virtual ProductEntity Product { get; set; }
        public virtual ShoppingCartEntity ShoppingCart { get; set; }
        public int Amount { get; set; }
    }
}
