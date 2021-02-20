using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SklepInternetowy.Authentication;
using SklepInternetowy.DatabaseManager;
using SklepInternetowy.Entities;
using SklepInternetowy.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SklepInternetowy.Controllers
{

    [ApiController]
    [Route("api/[controller]/[action]")]
    public class AuthController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly DBManager _dBManger;

        public AuthController(UserManager<ApplicationUser> userManager,DBManager dBManger, RoleManager<IdentityRole> roleManager, SignInManager<ApplicationUser> signInManager)
        {
            userManager.UserValidators.Clear();
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.signInManager = signInManager;
            _dBManger = dBManger;
        }


        [HttpPost]
        public async Task<IActionResult> Login(AuthLoginModel model)
        {
            if(userManager.FindByNameAsync(model.UserName).Result == null)
                return NotFound(new Response() { Success = false, Message = "Nie znaleziono takiego użytkownika" });

            var result = await signInManager.PasswordSignInAsync(model.UserName, model.Password, false, false);
            
            if (!result.Succeeded)
                return NotFound(new Response() { Success = false, Message = "Nie znaleziono takiego użytkownika" });
            
            return Ok(new Response() { Success = true, Message = "Udało się zalogować"});
        }

        [HttpPost]
        public async Task<IActionResult> RegisterAsync(AuthRegisterModel model)
        {

            if (userManager.FindByNameAsync(model.RegUserName).Result != null)
                return NotFound(new Response() { Success = false, Message = "Użytkownik o tej nazwie już istnieje" });

            if (userManager.FindByEmailAsync(model.Email).Result != null)
                return NotFound(new Response() { Success = false, Message = "Użytkownik o tym emailu już istnieje" });


            ShoppingCartEntity shoppingCartEntity = new ShoppingCartEntity() { };
            _dBManger.Add(shoppingCartEntity);
            var user = new ApplicationUser { UserName = model.RegUserName, Email = model.Email, Name = model.Name, Lastname = model.Lastname, ShoppingCart = shoppingCartEntity };
            var result = await userManager.CreateAsync(user, model.RegPassword);
            if (result.Succeeded)
            {
                await signInManager.SignInAsync(user, isPersistent: false);
                return Ok(new Response() { Success = true, Message = "Udało się zarejestrować"});
            }

            return Unauthorized(new Response() { Success = false, Message = "Wystąpił błąd skontaktuj się z administratorem" });
        }

        public async Task<IActionResult> Logout()
        {
            if (!User.Identity.IsAuthenticated)
                return Unauthorized(new Response() { Success = false, Message = "Funkcja dostępna tylko dla zalogowanych użytkowników" });

            await signInManager.SignOutAsync();

            return Ok(new Response() { Success = true, Message = "Udało się wylogować"});
        }
    }
}
