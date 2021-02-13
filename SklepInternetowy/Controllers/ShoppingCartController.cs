using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SklepInternetowy.Authentication;
using SklepInternetowy.DatabaseManager;

namespace SklepInternetowy.Controllers
{
    public class ShoppingCartController : Controller
    {
        public ShoppingCartController()
        {

        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
