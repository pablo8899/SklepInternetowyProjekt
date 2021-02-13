using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SklepInternetowy.Authentication;
using SklepInternetowy.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SklepInternetowy.DatabaseManager
{
    public class DBManager
    {
        public string Name { get { return GetUser()?.Name; } }
        public string LastName { get { return GetUser()?.Lastname; } }
        public bool Authenticated { get { return User.Identity.IsAuthenticated; } }
        public bool IsProductTableEmpty { get { return _dbContext.Products.Count() == 0; } }

        private readonly IHttpContextAccessor _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public ApplicationDbContext _dbContext;
        private ClaimsPrincipal User { get; }
        public DBManager(IHttpContextAccessor context, UserManager<ApplicationUser> userManager, ApplicationDbContext dbContext)
        {
            _context = context;
            _userManager = userManager;
            User = _context.HttpContext.User;
            _dbContext = dbContext;
        }

        public ApplicationUser GetUser()
        {
            if (!User.Identity.IsAuthenticated)
                return null;

            return _userManager.FindByNameAsync(_context.HttpContext.User.Identity.Name).Result;
        }

        public void Add(object item)
        {
            _dbContext.Add(item);
            _dbContext.SaveChanges();
        }
        public void AddRange(IEnumerable<object> list)
        {
            _dbContext.AddRange(list);
            _dbContext.SaveChanges();
        }

        public void Update(object item)
        {
            _dbContext.Update(item);
            _dbContext.SaveChanges();
        }

        public void Delete(object item)
        {
            _dbContext.Remove(item);
            _dbContext.SaveChanges();
        }

        public void AddTestData()
        {

            ShippingEntity[] shipping = new ShippingEntity[]
            {
                new ShippingEntity(){ Delivery = "Wysyłka standardowa", Price = 5 },
                new ShippingEntity(){ Delivery = "Wysyłka kurierska", Price = 15 },

            };
            AddRange(shipping);


            PromoCodeEntity[] PromoCodes = new PromoCodeEntity[]
            {
                new PromoCodeEntity(){ Code = "FIRST", DiscountPercent = 10 },
                new PromoCodeEntity(){ Code = "JANUARY2021", DiscountPercent = 2 },
                new PromoCodeEntity(){ Code = "FEBUARY2021", DiscountPercent = 20 },
                new PromoCodeEntity(){ Code = "COVID-19", DiscountPercent = 19 },
            };
            AddRange(PromoCodes);

            CategoriesEntity zegarki = new CategoriesEntity() { CategoryPath = "Zegarki" };
            CategoriesEntity koszulki = new CategoriesEntity() { CategoryPath = "Koszulki" };
            CategoriesEntity czapki = new CategoriesEntity() { CategoryPath = "Czapki" };
            CategoriesEntity okulary = new CategoriesEntity() { CategoryPath = "Okulary" };

            AddRange(new CategoriesEntity[] { zegarki, koszulki, czapki, okulary });

            Add(new ProductEntity()
            {
                Name = "Rolex DATEJUST 41",
                Amount = 10,
                Price = 7500.0,
                Image = "https://content.rolex.com/dam/2020//upright-bba-with-shadow/m126333-0010.png",
                Category = zegarki
            });


            Add(new ProductEntity()
            {
                Name = "Rolex GMT-MASTER II",
                Amount = 10,
                Price = 6900.0,
                Image = "https://content.rolex.com/dam/2020//upright-bba-with-shadow/m126711chnr-0002.png",
                Category = zegarki
            });

            Add(new ProductEntity()
            {
                Name = "Rolex DAY-DATE 40",
                Amount = 1,
                Price = 37000.0,
                Image = "https://content.rolex.com/dam/2020//upright-bba-with-shadow/m228239-0033.png",
                Category = zegarki
            });

            Add(new ProductEntity()
            {
                Name = "Koszulka Polo Ralph Lauren 41",
                Amount = 13,
                Price = 189.0,
                Image = "https://lb5.dstatic.pl/images/product-thumb/104689474-t-shirt-ralph-lauren-gladki-turkusowy-polo-ralph-lauren-l-promocja-royal-shop.jpg",
                Category = koszulki
            });

            Add(new ProductEntity()
            {
                Name = "Koszulka Adriano Guinari",
                Amount = 11,
                Discount =25,
                Price = 89.0,
                Image = "https://lb0.dstatic.pl/images/product-thumb/85404945-t-shirt-meski-adriano-guinari.jpg",
                Category = koszulki
            });

            Add(new ProductEntity()
            {
                Name = "Koszulka Polo Ralph Lauren 69",
                Amount = 18,
                Price = 339.0,
                Image = "https://lb0.dstatic.pl/images/product-thumb/103424166-t-shirt-meski-polo-ralph-lauren.jpg",
                Category = koszulki
            });

            Add(new ProductEntity()
            {
                Name = "Czapka z daszkiem Gucci Original GG",
                Amount = 18,
                Price = 1439.0,
                Image = "https://media.gucci.com/style/DarkGray_Center_0_0_800x800/1519402505/200035_KQWBG_9791_001_100_0000_Light-Original-GG-canvas-baseball-hat-with-Web.jpg",
                Category = czapki
            });

            Add(new ProductEntity()
            {
                Name = "Czapka z daszkiem Gucci Original GG Black",
                Amount = 18,
                Price = 1339.0,
                Image = "https://media.gucci.com/style/DarkGray_Center_0_0_800x800/1519416008/200035_KQWBG_1060_001_100_0000_Light-Original-GG-canvas-baseball-hat-with-Web.jpg",
                Category = czapki
            });

            Add(new ProductEntity()
            {
                Name = "Czapka z daszkiem Gucci Original GG Floral Paint",
                Amount = 18,
                Price = 1239.0,
                Image = "https://media.gucci.com/style/DarkGray_Center_0_0_800x800/1606935603/200035_4HADV_1000_001_100_0000_Light-Ken-Scott-print-canvas-baseball-hat.jpg",
                Category = czapki
            });

            Add(new ProductEntity()
            {
                Name = "Okulary Rayban hexagonal flat lenses",
                Amount = 18,
                Price = 502.99,
                Image = "https://assets.ray-ban.com/is/image/RayBan/8056597213097_shad_qt?$594$",
                Category = okulary
            });

            Add(new ProductEntity()
            {
                Name = "Okulary RB7151 HEXAGONAL OPTICS",
                Amount = 18,
                Price = 780.0,
                Image = "https://assets.ray-ban.com/is/image/RayBan/8053672915228_shad_qt?$594$",
                Category = okulary
            });

            Add(new ProductEntity()
            {
                Name = "Okulary Rayban RB2180V OVAL LENSES",
                Amount = 18,
                Price = 1780.0,
                Image = "https://assets.ray-ban.com/is/image/RayBan/8053672356878_shad_qt?$594$",
                Category = okulary
            });

            _dbContext.SaveChanges();
        }


        public ShoppingCartEntity GetShoppingCart()
        {
            if (!Authenticated)
                return null;

            return GetUser().ShoppingCart;
        }

        public List<ShoppingCartItemEntity> GetUserShoppingCartItems()
        {
            if (!Authenticated)
                return new List<ShoppingCartItemEntity>();

            var list = _dbContext.ShoppingCartItems.ToList().Where(x => x.ShoppingCart == GetShoppingCart()).ToList();

            return list;
        }

        public ProductEntity GetProductByID(int id)
        {
           return  _dbContext.Products.Where(x => x.Id == id).FirstOrDefault();
        }
        public PromoCodeEntity GetPromoCode(string code)
        {
            return _dbContext.PromoCodes.Where(x => x.Code == code).FirstOrDefault();
        }
        public List<ShippingEntity> GetShipping()
        {
            if (!Authenticated)
                return new List<ShippingEntity>();

            return _dbContext.Shipping.ToList();
        }
        public List<ShoppingHistoryEntity> GetShoppingHistory()
        {
            if (!Authenticated)
                return new List<ShoppingHistoryEntity>();

            return _dbContext.ShoppingHistories.ToList().Where(x => x.User == GetUser()).ToList();
        }


        public List<ShoppingCartItemEntity> GetUserShoppingCartItems(ShoppingCartEntity shoppingCart)
        {
            if (!Authenticated)
                return new List<ShoppingCartItemEntity>();

            var list = _dbContext.ShoppingCartItems.ToList().Where(x => x.ShoppingCart == shoppingCart).ToList();

            return list;
        }
    }
}
