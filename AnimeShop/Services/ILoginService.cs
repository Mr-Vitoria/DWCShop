using AnimeShop.Models;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace AnimeShop.Services
{
    public interface ILoginService
    {
        Task<Tuple<bool, string>> AddUser(HttpContext httpContext, User user);
        Task<Tuple<bool, string>> UpdateUser(HttpContext httpContext, User user);
        void SignOut(HttpContext httpContext);
        Task<Tuple<bool, string>> CheckUser(HttpContext httpContext, User user);
    }
}
