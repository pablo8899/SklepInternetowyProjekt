using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SklepInternetowy.Entities
{
    public class ProductEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int? Discount { get; set; }
        public virtual CategoriesEntity Category { get; set; }
        public double Price { get; set; }
        public string Image { get; set; }
        public int Amount { get; set; }
        public double TotalPrice 
        { 
            get
            {
                return Price - (Price * (Discount == null ? 0 : ((int)Discount * 0.01)));
            }
                
        }
    }
}
