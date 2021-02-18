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
            RuleFor(x => x.RegUserName).NotEmpty().WithMessage("Pole 'Username' nie może być puste"); ;
            RuleFor(x => x.Name).NotEmpty().WithMessage("Pole 'Name' nie może być puste"); ;
            RuleFor(x => x.Lastname).NotEmpty().WithMessage("Pole 'Lastname' nie może być puste"); ;
            RuleFor(x => x.Email).NotEmpty().EmailAddress().WithMessage("Pole 'Email' nie zawiera poprawnego adresu email");
            RuleFor(x => x.RegPassword).MinimumLength(5).WithMessage("Pole 'Password' musi zawierać przynajmniej 5 znaków");
            RuleFor(x => x.PasswordConfirmation).Equal(x => x.RegPassword).WithMessage("Hasła muszą być identczne");
        }
    }
}
