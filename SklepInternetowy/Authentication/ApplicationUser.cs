using Microsoft.AspNetCore.Identity;
using SklepInternetowy.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SklepInternetowy.Authentication
{
    public class ApplicationUser : IdentityUser
    {
        public override string UserName { get; set; }
        public virtual ShoppingCartEntity ShoppingCart { get; set; }
        public string Name { get; set; }
        public string Lastname { get; set; }
    }
}
