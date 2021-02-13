using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SklepInternetowy.Models
{
    public class AuthLoginModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
