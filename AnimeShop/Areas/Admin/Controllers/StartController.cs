using AnimeShop.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AnimeShop.Areas.Admin.Controllers
{

    [Area("Admin")]
    public class StartController : Controller
    {
        // GET: StartController
        public ActionResult Start()
        {
            return View();
        }
    }
}
