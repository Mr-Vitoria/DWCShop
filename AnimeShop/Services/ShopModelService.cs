using AnimeShop.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Concurrent;

namespace AnimeShop.Services
{
    public class ShopModelService : IShopModelService
    {
        ShopDBContext context;
        private static ConcurrentQueue<Product> RecentProducts = new ConcurrentQueue<Product>();
        public ShopModelService(ShopDBContext _context)
        {
            context = _context;
        }
        public async Task<ShopViewModel> CreateModel(HttpContext httpcontext)
        {
            ShopViewModel model = new ShopViewModel();
            model.Categories = context.Categories.ToList();
            model.Products = context.ProductsList.ToList();

            model.minPrice = (int)model.Products.Min(x => x.Price);
            model.maxPrice = (int)model.Products.Max(x => x.Price);

            var UserId = httpcontext.Request.Cookies["UserId"];
            if (UserId != null)
            {
                model.User = await context.Users.FirstOrDefaultAsync(x => x.Id == int.Parse(UserId));
                Order userOrder = await context.Orders
                                        .Include(x => x.User)
                                        .Include(x => x.OrderProducts)
                                        .FirstOrDefaultAsync(x => x.User.Id == model.User.Id);
                List<FavoriteProduct> FavPr = await context.FavoriteProducts
                                        .Include(x => x.User)
                                        .Include(x => x.Product)
                                        .Where(x => x.UserId == model.User.Id).ToListAsync();
                model.FavoriteProducts = FavPr.Select(x=>x.Product).ToList();
                if (userOrder == null)
                {
                    ShopViewModel.CountItemCart = 0;
                }
                else
                {
                    ShopViewModel.CountItemCart = userOrder.OrderProducts.Count();
                }

                if (FavPr == null)
                {
                    ShopViewModel.CountItemFacourites = 0;
                }
                else
                {
                    ShopViewModel.CountItemFacourites = FavPr.Count;
                }
                model.Status = null;
                model.RecentProducts = RecentProducts;
            }
            else
            {
                model.Status = "Чтобы создавать заказы и добавлять товары в избранное, зарегистрируйтесь или войдите в аккаунт!!!";
            }


            return model;
        }

        public void UpdateProductByParam(string seacrhTitle, int? CategoryId, int? minPrice,int? maxPrice, ref ShopViewModel model)
        {
            IEnumerable<Product> productsList = context.ProductsList.Include(x => x.Category);
            if (seacrhTitle != null)
            {
                model.SearchTitle = seacrhTitle;
                productsList = productsList.Where(x => x.Title.ToLower().Contains(seacrhTitle.ToLower()));
            }
            if (CategoryId != null)
            {
                model.CategoryID = (int)CategoryId;
                productsList = productsList.Where(x => x.CategoryId == CategoryId);
            }
            if (minPrice != null)
            {
                model.minPriceProd = (int)minPrice;
                productsList = productsList.Where(x => x.Price >= minPrice);
            }
            if (maxPrice != null)
            {
                model.maxPriceProd = (int)maxPrice;
                productsList = productsList.Where(x => x.Price <= maxPrice);
            }
            model.Products = productsList.ToList();
        }

        public async Task<Tuple<bool, string>> AddRecentProduct(HttpContext httpContext,int id)
        {

            var UserId = httpContext.Request.Cookies["UserId"];
            if (UserId == null)
            {
                return new Tuple<bool, string>(false, "User not login");

            }
            Product pr = await context.ProductsList.FirstOrDefaultAsync(x => x.Id == id);

            if(pr==null)
            {
                return new Tuple<bool, string>(false, "Product not found");
            }

            if (!RecentProducts.Contains(pr))
            {
                RecentProducts.Enqueue(pr);
                if (RecentProducts.Count > 8)
                {
                    RecentProducts.TryDequeue(out Product trash);
                }
            }
            return new Tuple<bool, string>(true, null);
        }
    }
}
