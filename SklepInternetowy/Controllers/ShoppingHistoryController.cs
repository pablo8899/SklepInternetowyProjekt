using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SklepInternetowy.DatabaseManager;
using SklepInternetowy.Entities;

namespace SklepInternetowy.Controllers
{
    public class ShoppingHistoryController : Controller
    {
        private readonly DBManager _dBManager;
        public ShoppingHistoryController(DBManager dBManager)
        {
            _dBManager = dBManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Route("ShoppingHistory/{id}")]
        public IActionResult Details(int id)
        {

            return View(_dBManager._dbContext.ShoppingHistories.Where(x => x.Id == id).FirstOrDefault());
        }
    }
}
