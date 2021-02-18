using System;

namespace SklepInternetowy.Models
{
    public class AuthRegisterModel
    {
        public string RegUserName { get; set; }
        public string Name { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }
        public string RegPassword { get; set; }
        public string PasswordConfirmation { get; set; }
    }
}
