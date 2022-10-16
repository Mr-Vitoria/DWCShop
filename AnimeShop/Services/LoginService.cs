using AnimeShop.Models;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;

namespace AnimeShop.Services
{
    public class LoginService : ILoginService
    {
        private readonly ShopDBContext context;

        public LoginService(ShopDBContext _context)
        {
            context = _context;
        }
        public async Task<Tuple<bool, string>> AddUser(HttpContext httpContext, User user)
        {
            User us = await context.Users.FirstOrDefaultAsync(us => us.Phone == user.Phone || us.Login == user.Login);
            if (us == null)
            {
                context.Users.Add(user);
                await context.SaveChangesAsync();
                httpContext.Response.Cookies.Append("Login", user.Login);
                httpContext.Response.Cookies.Append("UserId", user.Id.ToString());
                return new Tuple<bool, string>(true, $"Здравствуйте,{user.Login}");
            }
            return new Tuple<bool, string>(false, "User is already registered");
        }

        public async Task<Tuple<bool, string>> CheckUser(HttpContext httpContext, User user)
        {
            User us = await context.Users.FirstOrDefaultAsync(us => us.Phone == user.Phone);
            if (us == null)
            {
                return new Tuple<bool, string>(false, "User not found");
            }

            if (us.Password != user.Password)
            {
                return new Tuple<bool, string>(false, "Password incorrect");
            }
            else
            {
                httpContext.Response.Cookies.Append("Login", us.Login);
                httpContext.Response.Cookies.Append("UserId", us.Id.ToString());
                // var str = HttpContext.Request.Cookies["name"]
                return new Tuple<bool, string>(true, $"Здравствуйте,{us.Login}");
            }
        }

        public void SignOut(HttpContext httpContext)
        {
            httpContext.Response.Cookies.Delete("Login");
            httpContext.Response.Cookies.Delete("UserId");


        }

        public async Task<Tuple<bool, string>> UpdateUser(HttpContext httpContext, User user)
        {
            try
            {
                context.Users.Update(user);
                await context.SaveChangesAsync();
                httpContext.Response.Cookies.Append("Login", user.Login);
                httpContext.Response.Cookies.Append("UserId", user.Id.ToString());
                return new Tuple<bool, string>(true, $"Вы успешно обовили профиль,{user.Login}");
            }
            catch
            {
                return new Tuple<bool, string>(false, "Error update user");

            }
        }
    }
}
