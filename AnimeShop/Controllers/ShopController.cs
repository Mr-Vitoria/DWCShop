using AnimeShop.Models;
using AnimeShop.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace AnimeShop.Controllers
{
    public class ShopController : Controller
    {
        private readonly ShopDBContext context;
        private readonly IShopService shopService;
        private readonly IShopModelService shopModelService;
        private readonly IMailService mailService;

        public ShopController(ShopDBContext context,IShopService shopService,IShopModelService shopModelService,IMailService mailService)
        {
            this.context = context;
            this.shopService = shopService;
            this.shopModelService = shopModelService;
            this.mailService = mailService;
        }
        [HttpGet]
        public async Task<IActionResult> ShopCart(string returnURL)
        {
            var tuple = await shopService.GetUserOrder(HttpContext);
            if(tuple.Item1)
            {
                return View(tuple.Item3);
            }

            TempData["ModelStatus"] = tuple.Item2;

            if (returnURL == null)
            {
                return RedirectToAction("Index", "Home");
            }

            return Redirect(returnURL);
        }


        [HttpPost]
        [ActionName("ShopCart")]
        public IActionResult ConfirmShopCart()
        {
            return View();
        }


        public async Task<IActionResult> AddToCart(int id, string returnURL,int amount = 1)
        {
            var tuple = await shopService.AddProductInCart(HttpContext,id,amount);
            if (!tuple.Item1)
            {
                TempData["ModelStatus"] = tuple.Item2;
            }


            if (returnURL == null)
            {
                return RedirectToAction("ShopCart", "Shop");
            }

            return Redirect(returnURL);
        }
        public async Task<IActionResult> AddCountProduct(int id,string returnURL)
        {
            var tuple = await shopService.AddProductInCart(HttpContext, id, 1);
            if (!tuple.Item1)
            {
                TempData["ModelStatus"] = tuple.Item2;
            }


            if (returnURL == null)
            {
                return RedirectToAction("ShopCart", "Shop");
            }

            return Redirect(returnURL);
        }

       
        public async Task<IActionResult> RemoveProduct(int id,string returnURL)
        {
            var tuple = await shopService.RemoveProductFromCart(HttpContext, id);
            if (!tuple.Item1)
            {
                TempData["ModelStatus"] = tuple.Item2;
            }


            if (returnURL == null)
            {
                return RedirectToAction("ShopCart", "Shop");
            }

            return Redirect(returnURL);
        }
        public async Task<IActionResult> RemoveCountProduct(int id,string returnURL)
        {
            var tuple = await shopService.AddProductInCart(HttpContext, id, -1);
            if (!tuple.Item1)
            {
                TempData["ModelStatus"] = tuple.Item2;
            }


            if (returnURL == null)
            {
                return RedirectToAction("ShopCart", "Shop");
            }

            return Redirect(returnURL);
        }



        [HttpGet]
        public async Task<IActionResult> Favourites(string returnURL)
        {
            var tuple = await shopService.GetFavoriteProducts(HttpContext);
            if (tuple.Item1)
            {
                return View(tuple.Item3);
            }

            TempData["ModelStatus"] = tuple.Item2;

            if (returnURL == null)
            {
                return RedirectToAction("Index", "Home");
            }

            return Redirect(returnURL);
        }
        public async Task<IActionResult> AddToFavourites(int id,string returnURL)
        {
            var tuple = await shopService.AddProductInFavourite(HttpContext, id);
            if (!tuple.Item1)
            {
                TempData["ModelStatus"] = tuple.Item2;
            }


            if (returnURL == null)
            {
                return RedirectToAction("ShopCart", "Shop");
            }

            return Redirect(returnURL);
        }
        public async Task<IActionResult> RemoveProductFromFeatur(int id)
        {
            var tuple = await shopService.RemoveProductFromFavourite(HttpContext, id);
            if (!tuple.Item1)
            {
                TempData["ModelStatus"] = tuple.Item2;
            }
            return RedirectToAction("Favourites", "Shop");
        }



        [HttpGet]
        public async Task<IActionResult> ProductDetail(int id,string returnUrl)
        {
            await shopModelService.AddRecentProduct(HttpContext, id);
            return View(await shopService.GetProductModelById(HttpContext, returnUrl, id));
        }
        [HttpGet]
        public async Task<IActionResult> ShopList(string seacrhTitle, int? CategoryId,int? minPrice,int? maxPrice, bool NeedSearch=true)
        {
            
            ShopViewModel model = await shopModelService.CreateModel(HttpContext);

            model.Categories = context.Categories.ToList();

            if(NeedSearch)
            {
                shopModelService.UpdateProductByParam(seacrhTitle,CategoryId,minPrice,maxPrice,ref model);
            }
            else
            {
                model.SearchTitle = null;
                model.CategoryID = 0;
            }


            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> CheckOut()
        {
            var tuple = await shopService.GetUserOrder(HttpContext);
            if (!tuple.Item1)
            {
                TempData["ModelStatus"] = tuple.Item2;
            }
            return View(tuple.Item3);
        }

        [HttpPost]
        public async Task<IActionResult> SendEmail()
        {
            var tuple = await shopService.GetUserOrder(HttpContext);
            if (!tuple.Item1)
            {
                TempData["ModelStatus"] = tuple.Item2;
            }
            else
            {
                await mailService.SendAsync(tuple.Item3,"Order from DWC Shop");
                await shopService.ClearOrder(tuple.Item3.User.Id);
            }
            return RedirectToAction("Index", "Home");
        }


        public async Task<IActionResult> ProductModal(int id)
        {
            Product product;
            product = await shopService.GetProductById(id);


            return PartialView("_CartModelPartial", product);
        }
    }
}
