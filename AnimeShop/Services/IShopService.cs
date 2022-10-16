using AnimeShop.Models;

namespace AnimeShop.Services
{
    public interface IShopService
    {
        Task<Tuple<bool, string, Order>> GetUserOrder(HttpContext httpContext);
        Task ClearOrder(int UserId);


        Task<Tuple<bool, string>> AddProductInCart(HttpContext httpContext, int id, int amount);
        Task<Tuple<bool, string>> RemoveProductFromCart(HttpContext httpContext, int id);
        Task<ProductViewModel> GetProductModelById(HttpContext httpContext, string returnURL, int id);


        Task<Tuple<bool, string, List<FavoriteProduct>>> GetFavoriteProducts(HttpContext httpContext);
        Task<Tuple<bool, string>> AddProductInFavourite(HttpContext httpContext, int id);
        Task<Tuple<bool, string>> RemoveProductFromFavourite(HttpContext httpContext, int id);

        Task<Product> GetProductById(int id);


    }
}
