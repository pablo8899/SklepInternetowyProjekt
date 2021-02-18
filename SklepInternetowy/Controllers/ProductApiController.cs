using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SklepInternetowy.DatabaseManager;
using SklepInternetowy.Entities;
using SklepInternetowy.Models;

namespace SklepInternetowy.Controllers
{

    [ApiController]
    [Route("api/[controller]/[action]")]
    public class ProductApiController : Controller
    {
        public readonly DBManager _dBManager;
        public ProductApiController(DBManager dBManager)
        {
            _dBManager = dBManager;
        }
        [HttpPost]
        public IActionResult AddToCart(ProductModel model)
        {
            if (!_dBManager.Authenticated)
                return Unauthorized(new Response() { Sucess = false, Message = "By dodać produkt do koszyka musisz się zalogować" });

            ProductEntity product = _dBManager.GetProductByID(model.ProductID);
            if (product == null)
                return NotFound(new Response() { Sucess = false, Message = "Nie znaleziono takiego produktu" });

            if(model.Amount < 0)
                return StatusCode(406, new Response() { Sucess = false, Message = "Ilość towaru musi być większa od 0" });

            ShoppingCartItemEntity shopCartItem = _dBManager.GetUserShoppingCartItems().Where(x => x.Product == product).FirstOrDefault();

            if (product.Amount < (shopCartItem == null ? 0 : shopCartItem.Amount) + model.Amount)
                return StatusCode(406, new Response() { Sucess = false, Message = "Niewystarczająca ilość towaru" });



            if (shopCartItem == null)
            {
                shopCartItem = new ShoppingCartItemEntity()
                {
                    ShoppingCart = _dBManager.GetShoppingCart(),
                    Product = product,
                    Amount = model.Amount
                };
                _dBManager.Add(shopCartItem);
            } else
            {
                shopCartItem.Amount += model.Amount;
                _dBManager.Update(shopCartItem);
            }

            return Ok(new Response() { Sucess = true, Message = "Poprawnie dodano produkt do koszyka" });
        }

        [HttpPost]
        public IActionResult BuyNow(ProductModel model)
        {
            return null;
        }

        [HttpPost]
        public IActionResult AddToFavorite(ProductModel model)
        {
            if (!_dBManager.Authenticated)
                return Unauthorized(new Response() { Sucess = false, Message = "By dodać produkt do ulubionych musisz się zalogować" });

            ProductEntity product = _dBManager.GetProductByID(model.ProductID);

            if(_dBManager.IsProductFavorite(product))
            {
                var fp = _dBManager.GetUserFavoriteProducts().Where(x => x.Product == product).FirstOrDefault();
                _dBManager.Delete(fp);

                return Ok(new Response() { Sucess = true, Message = "Usunięto z ulubionych" });
            } else
            {
                var f = new FavoriteProductEntity() { Product = product, User = _dBManager.GetUser() };
                _dBManager.Add(f);

                return Ok(new Response() { Sucess = true, Message = "Dodano do ulubionych" });
            }
        }

        [HttpGet]
        public IActionResult GetShoppingCartCount()
        {
            if(!_dBManager.Authenticated)
                return Unauthorized(new Response() { Sucess = false, Message = "Nie zalogowano" });

            return Ok(new Response() { Sucess = true, Message = _dBManager.GetUserShoppingCartItems().Sum(x => x.Amount).ToString() });
        }
    }
}
