using FluentValidation;
using SklepInternetowy.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SklepInternetowy.Validators
{
    public class AuthRegisterValidator : AbstractValidator<AuthRegisterModel>
    {
        public AuthRegisterValidator()
        {
            RuleFor(x => x.UserName).NotEmpty();
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Lastname).NotEmpty();
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
            RuleFor(x => x.Password).NotEmpty().MinimumLength(5);
            RuleFor(x => x.PasswordConfirmation).Equal(x => x.Password).WithMessage("Hasła muszą być identczne");
        }
    }
}
