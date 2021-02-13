using Microsoft.AspNetCore.Mvc;
using SklepInternetowy.DatabaseManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SklepInternetowy.ViewComponents
{
    public class ShoppingCartItemsCount : ViewComponent
    {
        public readonly DBManager _dBManager;
        public ShoppingCartItemsCount(DBManager dBManager)
        {
            _dBManager = dBManager;
        }

        public IViewComponentResult Invoke()
        {
            ViewBag.ShoppingCartItemsCount =  _dBManager.GetUserShoppingCartItems().Sum(x => x.Amount);
            return View("Index");
        }
    }
}
