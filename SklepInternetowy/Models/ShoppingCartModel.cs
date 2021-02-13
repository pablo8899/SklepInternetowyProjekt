using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SklepInternetowy.Models
{
    public class ShoppingCartModel
    {
        public int ProductID { get; set; }
        public int? Amount { get; set; }
        public string Code { get; set; }
    }
}
