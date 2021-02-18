using SklepInternetowy.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SklepInternetowy.Entities
{
    public class FavoriteProductEntity
    {
        public int Id { get; set; }
        public virtual ApplicationUser User { get; set; }
        public virtual ProductEntity Product { get; set; }
    }
}
