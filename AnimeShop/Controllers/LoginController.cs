using AnimeShop.Helpers;
using AnimeShop.Models;
using AnimeShop.Services;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.Linq;

namespace AnimeShop.Controllers
{
    public class LoginController : Controller
    {
        private readonly ShopDBContext context;
        private readonly ILoginService loginService;
        private readonly IMailService mailService;

        public LoginController(ShopDBContext _context,ILoginService loginService,IMailService mailService)
        {
            context = _context;
            this.loginService = loginService;
            this.mailService = mailService;
        }


        //Register

        [HttpGet]
        public IActionResult Register()
        {
            return View(new User());
        }

        [HttpPost]
        [ActionName("Register")]
        public async Task<IActionResult> ConfirmRegister(User user, string confirmPassword)
        {
            if (!ModelState.IsValid || user.Password != confirmPassword)
            {
                TempData["ModelStatus"] = "Неправильно указан "+ModelState.Values.FirstOrDefault(x=>x.Errors.Count>0).RawValue;
                return View("Register", new User
                {
                    Phone = user.Phone,
                    FirstName = user.FirstName,
                    SurName = user.SurName,
                    Login = user.Login
                });
            }

            Tuple<bool, string> tuple = await loginService.AddUser(HttpContext,user);

            TempData["ModelStatus"] = tuple.Item2;
            if (tuple.Item1)
            {
                await mailService.SendAsync(user, "Thank for registration on DWC Shop");
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return View("Register", new User
                {
                    Phone = user.Phone,
                    FirstName = user.FirstName,
                    SurName = user.SurName,
                    Login = user.Login
                });
            }


        }



        //Login
        [HttpGet]
        public IActionResult Login()
        {

            return View(new User());
        }

        [HttpPost]
        [ActionName("Login")]
        public async Task<IActionResult> ConfirmLogin(User user)
        {
            Tuple<bool, string> tuple = await loginService.CheckUser(HttpContext, user);
            TempData["ModelStatus"] = tuple.Item2;
            if (tuple.Item1)
            {
                return RedirectToAction("Index", "Home");
            }

            return View("Login", new User { Phone = user.Phone });
        }

        //Profile
        [HttpGet]
        public IActionResult Profile()
        {
            return View(context.Users.FirstOrDefault(x=>x.Id== int.Parse(HttpContext.Request.Cookies["UserId"])));
        }
        [HttpGet]
        public IActionResult EditProfile(int id)
        {
            return View(context.Users.FirstOrDefault(x=>x.Id== id));
        }
        [HttpPost]
        public async Task<IActionResult> EditProfile(User us, IFormFile ImgUrl)
        {

            if (ImgUrl != null)
            {
                await FileUploaderHelper.DeleteImg(us.ImageURl);
                us.ImageURl = await FileUploaderHelper.Upload(ImgUrl);

            }
            Tuple<bool, string> tuple = await loginService.UpdateUser(HttpContext, us);
            TempData["ModelStatus"] = tuple.Item2;
            if (tuple.Item1)
            {
                return RedirectToAction("Profile", "Login");
            }

            return RedirectToAction("Profile", "Login");
        }

        //Sign Out

        public new IActionResult SignOut()
        {
            loginService.SignOut(HttpContext);
            TempData["ModelStatus"] = "You have logged out of your account";
            return RedirectToAction("Index", "Home");
        }
    }
}
