using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SklepInternetowy.Authentication;
using SklepInternetowy.DatabaseManager;
using SklepInternetowy.Entities;
using SklepInternetowy.Models;

namespace SklepInternetowy.Controllers
{

    [ApiController]
    [Route("api/[controller]/[action]")]
    public class ShoppingCartApiController : Controller
    {
        public readonly DBManager _dBManager;
        public ShoppingCartApiController(DBManager dBManager)
        {
            _dBManager = dBManager;
        }

        [HttpPost]
        public IActionResult ChangeAmountOfProduct(ShoppingCartModel model)
        {
            if (!_dBManager.Authenticated)
                return Unauthorized(new Response() { Success = false, Message = "By zmienić ilość produktu musisz się zalogować" });

            if (model.Amount != 1 && model.Amount != -1)
                return StatusCode(406, new Response() { Success = false, Message = "Ilość zmniejszanego towaru musi wynosić 1 lub -1" });

            ProductEntity product = _dBManager.GetProductByID(model.ProductID);
            if (product == null)
                return NotFound(new Response() { Success = false, Message = "Nie znaleziono takiego produktu" });


            ShoppingCartItemEntity shopCartItem = _dBManager.GetUserShoppingCartItems().Where(x => x.Product == product).FirstOrDefault();
            if (shopCartItem == null)
                return NotFound(new Response() { Success = false, Message = "Nie znaleziono takiego produktu w koszyku" });

            if (model.Amount == 1)
                if (product.Amount < shopCartItem.Amount + model.Amount)
                    return StatusCode(406, new Response() { Success = false, Message = "Niewystarczająca ilość towaru" });


            ShoppingCartEntity shoppingCart = _dBManager.GetShoppingCart();

            int count = shopCartItem.Amount + (int)model.Amount;

            string summaryPrice = (count * shopCartItem.Product.TotalPrice).ToString("C2");
            if (count == 0)
            {
                dynamic jsonObject = new JObject();

                _dBManager.Delete(shopCartItem);
                var shipping = shoppingCart.Shipping == null ? 0 : shoppingCart.Shipping.Price;
                double summaryTotalPriceObject = _dBManager.GetUserShoppingCartItems().Sum(x => x.Product.TotalPrice * x.Amount) + shipping;

                double discount = shoppingCart.PromoCode == null ? 0 : summaryTotalPriceObject * (shoppingCart.PromoCode.DiscountPercent * 0.01);

                if (shoppingCart.PromoCode != null)
                {
                    jsonObject.summaryBeforeDiscountPrice = summaryTotalPriceObject.ToString("C2");
                    jsonObject.summaryDiscountPercent = shoppingCart.PromoCode.DiscountPercent;
                    jsonObject.summaryDiscountPrice = discount.ToString("C2");
                }

                string summaryTotalPrice = (summaryTotalPriceObject - discount).ToString("C2");

                jsonObject.deleted = true;
                jsonObject.count = 0;
                jsonObject.summaryTotalPrice = summaryTotalPrice;
                jsonObject.summaryPrice = summaryPrice;


                return Ok(new Response() { Success = true, Message = JsonConvert.SerializeObject(jsonObject) });
            }
            else
            {
                dynamic jsonObject = new JObject();
                shopCartItem.Amount += (int)model.Amount;

                _dBManager.Update(shopCartItem);
                var shipping = shoppingCart.Shipping == null ? 0 : shoppingCart.Shipping.Price;
                double summaryTotalPriceObject = _dBManager.GetUserShoppingCartItems().Sum(x => x.Product.TotalPrice * x.Amount) + shipping;

                double discount = shoppingCart.PromoCode == null ? 0 : summaryTotalPriceObject * (shoppingCart.PromoCode.DiscountPercent * 0.01);

                if (shoppingCart.PromoCode != null)
                {
                    jsonObject.summaryBeforeDiscountPrice = summaryTotalPriceObject.ToString("C2");
                    jsonObject.summaryDiscountPercent = shoppingCart.PromoCode.DiscountPercent;
                    jsonObject.summaryDiscountPrice = discount.ToString("C2");
                }
                string summaryTotalPrice = (summaryTotalPriceObject - discount).ToString("C2");

                jsonObject.deleted = false;
                jsonObject.count = count;
                jsonObject.summaryTotalPrice = summaryTotalPrice;
                jsonObject.summaryPrice = summaryPrice;

                return Ok(new Response() { Success = true, Message = JsonConvert.SerializeObject(jsonObject) });
            }

        }

        [HttpPost]
        public IActionResult DeleteShoppingCartItem(ShoppingCartModel model)
        {
            if (!_dBManager.Authenticated)
                return Unauthorized(new Response() { Success = false, Message = "By usunąć produkt musisz się zalogować" });

            ProductEntity product = _dBManager.GetProductByID(model.ProductID);
            if (product == null)
                return NotFound(new Response() { Success = false, Message = "Nie znaleziono takiego produktu" });


            ShoppingCartItemEntity shopCartItem = _dBManager.GetUserShoppingCartItems().Where(x => x.Product == product).FirstOrDefault();
            ShoppingCartEntity shoppingCart = _dBManager.GetShoppingCart();

            if (shopCartItem == null)
                return NotFound(new Response() { Success = false, Message = "Nie znaleziono takiego produktu w koszyku" });

            _dBManager.Delete(shopCartItem);

            dynamic jsonObject = new JObject();

            var shipping = shoppingCart.Shipping == null ? 0 : shoppingCart.Shipping.Price;
            double summaryTotalPriceObject = _dBManager.GetUserShoppingCartItems().Sum(x => x.Product.TotalPrice * x.Amount) + shipping;

            double discount = shoppingCart.PromoCode == null ? 0 : summaryTotalPriceObject * (shoppingCart.PromoCode.DiscountPercent * 0.01);

            if (shoppingCart.PromoCode != null)
            {
                jsonObject.summaryBeforeDiscountPrice = summaryTotalPriceObject.ToString("C2");
                jsonObject.summaryDiscountPercent = shoppingCart.PromoCode.DiscountPercent;
                jsonObject.summaryDiscountPrice = discount.ToString("C2");
            }

            string summaryTotalPrice = (summaryTotalPriceObject - discount).ToString("C2");

            jsonObject.deleted = true;
            jsonObject.count = 0;
            jsonObject.summaryTotalPrice = summaryTotalPrice;
            jsonObject.summaryPrice = "";

            return Ok(new Response() { Success = true, Message = JsonConvert.SerializeObject(jsonObject) });

        }

        [HttpPost]
        public IActionResult ApplyPromoCode(ShoppingCartModel model)
        {

            if (!_dBManager.Authenticated)
                return Unauthorized(new Response() { Success = false, Message = "By skorzystać z kodu musisz się zalogować" });

            if (string.IsNullOrEmpty(model.Code))
                return StatusCode(406, new Response() { Success = false, Message = "Kod nie może być pusty" });

            var PromoCode = _dBManager.GetPromoCode(model.Code.Trim().ToUpper());
            if (PromoCode == null)
                return NotFound(new Response() { Success = false, Message = "Nie znaleziono takiego kodu" });

            var shoppingCart = _dBManager.GetShoppingCart();
            var ShoppingCartPromoCode = shoppingCart.PromoCode;
            if (ShoppingCartPromoCode != null)
                return StatusCode(406, new Response() { Success = false, Message = "Już zastosowano kod" });

            shoppingCart.PromoCode = PromoCode;
            _dBManager.Update(shoppingCart);


            dynamic jsonObject = new JObject();

            var shipping = shoppingCart.Shipping == null ? 0 : shoppingCart.Shipping.Price;
            double summaryTotalPriceObject = _dBManager.GetUserShoppingCartItems().Sum(x => x.Product.TotalPrice * x.Amount) + shipping;

            double discount = summaryTotalPriceObject * (shoppingCart.PromoCode.DiscountPercent * 0.01);

            string summaryTotalPrice = (summaryTotalPriceObject - discount).ToString("C2");

            jsonObject.summaryBeforeDiscountPrice = summaryTotalPriceObject.ToString("C2");
            jsonObject.summaryDiscountPercent = shoppingCart.PromoCode.DiscountPercent;
            jsonObject.summaryDiscountPrice = discount.ToString("C2");
            jsonObject.summaryTotalPrice = summaryTotalPrice;

            return Ok(new Response() { Success = true, Message = JsonConvert.SerializeObject(jsonObject) });
        }

        [HttpPost]
        public IActionResult ClearPromoCode()
        {

            if (!_dBManager.Authenticated)
                return Unauthorized(new Response() { Success = false, Message = "By skorzystać z kodu musisz się zalogować" });


            var shoppingCart = _dBManager.GetShoppingCart();
            var ShoppingCartPromoCode = shoppingCart.PromoCode;
            if (ShoppingCartPromoCode == null)
                return StatusCode(406, new Response() { Success = false, Message = "Brak kodu" });

            shoppingCart.PromoCode = null;
            _dBManager.Update(shoppingCart);


            dynamic jsonObject = new JObject();

            var shipping = shoppingCart.Shipping == null ? 0 : shoppingCart.Shipping.Price;
            double summaryTotalPriceObject = _dBManager.GetUserShoppingCartItems().Sum(x => x.Product.TotalPrice * x.Amount) + shipping;


            jsonObject.summaryTotalPrice = summaryTotalPriceObject.ToString("C2");

            return Ok(new Response() { Success = true, Message = JsonConvert.SerializeObject(jsonObject) });
        }

        [HttpPost]
        public IActionResult ChangeShippingOption(ShippingModel model)
        {

            if (!_dBManager.Authenticated)
                return Unauthorized(new Response() { Success = false, Message = "By wybrać opcję dostawy musisz się zalogować" });


            var shoppingCart = _dBManager.GetShoppingCart();
            var shipping = _dBManager.GetShipping().Where(x => x.Id == model.ID).FirstOrDefault();
            if (shipping == null)
                return StatusCode(406, new Response() { Success = false, Message = "Brak opcji dostawy" });

            shoppingCart.Shipping = shipping;
            _dBManager.Update(shoppingCart);


            dynamic jsonObject = new JObject();

            double summaryTotalPriceObject = _dBManager.GetUserShoppingCartItems().Sum(x => x.Product.TotalPrice * x.Amount) + shipping.Price;




            double discount = shoppingCart.PromoCode == null ? 0 : summaryTotalPriceObject * (shoppingCart.PromoCode.DiscountPercent * 0.01);

            if (shoppingCart.PromoCode != null)
            {
                jsonObject.summaryBeforeDiscountPrice = summaryTotalPriceObject.ToString("C2");
                jsonObject.summaryDiscountPercent = shoppingCart.PromoCode.DiscountPercent;
                jsonObject.summaryDiscountPrice = discount.ToString("C2");
            }

            string summaryTotalPrice = (summaryTotalPriceObject - discount).ToString("C2");

            jsonObject.summaryTotalPrice = summaryTotalPrice;

            return Ok(new Response() { Success = true, Message = JsonConvert.SerializeObject(jsonObject) });

        }


        [HttpPost]
        public IActionResult Pay()
        {

            if (!_dBManager.Authenticated)
                return Unauthorized(new Response() { Success = false, Message = "By zapłacić musisz się zalogować" });

            var shoppingCart = _dBManager.GetShoppingCart();
            var shoppingCartItems = _dBManager.GetUserShoppingCartItems();

            if (shoppingCartItems.Count == 0)
                return NotFound(new Response() { Success = false, Message = "Brak produktów w koszyku" });

            if (shoppingCart.Shipping == null)
                return NotFound(new Response() { Success = false, Message = "Wybierz opcję dostawy" });


            foreach (var item in shoppingCartItems)
            {
                item.Product.Amount -= item.Amount;
                _dBManager.Update(item.Product);
            }

            ShoppingHistoryEntity shoppingHistory = new ShoppingHistoryEntity() { ShoppingCart = shoppingCart, User = _dBManager.GetUser() };
            _dBManager.Add(shoppingHistory);

            ShoppingCartEntity shoppingCartEntity = new ShoppingCartEntity() { };
            _dBManager.Add(shoppingCartEntity);

            ApplicationUser user = _dBManager.GetUser();
            user.ShoppingCart = shoppingCartEntity;

            _dBManager.Update(user);

            return Ok(new Response() { Success = true, Message = "Gratulację zakupy powiodły się" });
        }
    }
}
