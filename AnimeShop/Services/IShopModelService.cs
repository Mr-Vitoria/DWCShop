using AnimeShop.Models;

namespace AnimeShop.Services
{
    public interface IShopModelService
    {
        Task<ShopViewModel> CreateModel(HttpContext httpcontext);
        void UpdateProductByParam(string seacrhTitle, int? CategoryId, int? minPrice,int? maxPrice, ref ShopViewModel model);
        public Task<Tuple<bool, string>> AddRecentProduct(HttpContext httpContext, int id);
    }
}
