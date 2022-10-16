using AnimeShop.Models;
using AnimeShop.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Text.Json;

namespace AnimeShop.Controllers
{
    public class HomeController : Controller
    {
        private readonly IShopModelService shopModelService;

        public HomeController(IShopModelService _shopModelService)
        {
            shopModelService = _shopModelService;
        }

        public async Task<IActionResult> Index()
        {

            var UserId = HttpContext.Request.Cookies["UserId"];
            ShopViewModel model = await shopModelService.CreateModel(HttpContext);

            if(model.FavoriteProducts!=null)
                model.FavoriteProducts = model.FavoriteProducts.Take(8).ToList();

            if(model.Status!=null)
                TempData["ModelStatus"] = model.Status;

            return View(model);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}