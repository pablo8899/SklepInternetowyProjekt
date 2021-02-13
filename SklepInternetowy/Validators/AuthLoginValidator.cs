using FluentValidation;
using SklepInternetowy.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SklepInternetowy.Validators
{
    public class AuthLoginValidator : AbstractValidator<AuthLoginModel>
    {
        public AuthLoginValidator()
        {
            RuleFor(x => x.UserName).NotEmpty();
            RuleFor(x => x.Password).NotEmpty(); 
        }
    }
}
