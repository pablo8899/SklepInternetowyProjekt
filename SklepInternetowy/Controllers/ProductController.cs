using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SklepInternetowy.DatabaseManager;
using SklepInternetowy.Models;

namespace SklepInternetowy.Controllers
{
    public class ProductController : Controller
    {
        private readonly DBManager _dBManager;
        public ProductController(DBManager dBManager)
        {
            _dBManager = dBManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Details(int id)
        {
            return View(id);
        }
    }
}
