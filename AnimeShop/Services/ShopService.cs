using AnimeShop.Models;
using Microsoft.EntityFrameworkCore;

namespace AnimeShop.Services
{
    public class ShopService : IShopService
    {

        private readonly ShopDBContext context;

        public ShopService(ShopDBContext context)
        {
            this.context = context;
        }

        public async Task<Tuple<bool, string>> AddProductInCart(HttpContext httpContext, int id, int amount)
        {
            var UserId = httpContext.Request.Cookies["UserId"];

            if (UserId == null)
            {
                return new Tuple<bool, string>(false, "User not login");

            }
            Product pr = await context.ProductsList.FirstOrDefaultAsync(x => x.Id == id);
            Order order = await context.Orders
                                .Include(x => x.User)
                                .Include(x => x.OrderProducts)
                                .ThenInclude(x => x.Product)
                                .FirstOrDefaultAsync(x => x.User.Id == int.Parse(UserId));
            if (order != null)
            {
                if (order.OrderProducts.Any(x => x.ProductId == id))
                {
                    OrderProducts ordPr = order.OrderProducts.First(x => x.ProductId == id);
                    if (ordPr.Amount + amount < 0)
                    {
                        return new Tuple<bool, string>(false, "Products cannot be less than 0");
                    }
                    ordPr.Amount += amount;
                    ordPr.TotalPrice = (int)(pr.Price *pr.Discount * ordPr.Amount);

                }
                else
                {
                    context.OrderProducts.Add(new OrderProducts()
                    {
                        Amount = amount,
                        Order = order,
                        OrderId = order.Id,
                        Product = pr,
                        ProductId = pr.Id,
                        TotalPrice = (int)(pr.Price *pr.Discount* amount)
                        
                    });

                }
                context.Orders.Update(order);
            }
            else
            {
                order = new Order();
                order.User = await context.Users.FirstOrDefaultAsync(x => x.Id == int.Parse(UserId));
                order.OrderProducts = new List<OrderProducts>();
                context.OrderProducts.Append(new OrderProducts()
                {
                    Amount = 1,
                    Order = order,
                    OrderId = order.Id,
                    Product = pr,
                    ProductId = pr.Id,
                    TotalPrice = (int)(pr.Price*pr.Discount)
                });
                await context.Orders.AddAsync(order);
            }

            int totalSumm = 0;
            foreach (var item in order.OrderProducts)
            {
                totalSumm += item.TotalPrice;
            }
            order.totalPrice = totalSumm;
            await context.SaveChangesAsync();
            return new Tuple<bool, string>(true, "OK");
        }

        public async Task<Tuple<bool, string>> AddProductInFavourite(HttpContext httpContext, int id)
        {
            var UserId = httpContext.Request.Cookies["UserId"];
            if (UserId == null)
            {
                return new Tuple<bool, string>(false, "User not login");
            }

            Product pr = await context.ProductsList.FirstOrDefaultAsync(x => x.Id == id);
            User us = await context.Users.FirstOrDefaultAsync(x => x.Id == int.Parse(UserId));
            FavoriteProduct Favpr = await context.FavoriteProducts.FirstOrDefaultAsync(
                                                x => x.UserId == us.Id
                                                && x.ProductId == pr.Id);
            if (Favpr != null)
            {
                context.FavoriteProducts.Remove(Favpr);
            }
            else
            {
                await context.FavoriteProducts.AddAsync(new FavoriteProduct()
                {
                    Product = pr,
                    ProductId = pr.Id,
                    User = us,
                    UserId = us.Id
                });

            }

            await context.SaveChangesAsync();
            return new Tuple<bool, string>(true, "OK");
        }

        public async Task ClearOrder(int UserId)
        {
            Order UserOrder = await context.Orders.FirstOrDefaultAsync(x => x.User.Id == UserId);
            if(UserOrder!=null)
            {
                context.OrderProducts.RemoveRange(context.OrderProducts.Where(x => x.OrderId == UserOrder.Id));
                context.Orders.Remove(UserOrder);
            }
            await context.SaveChangesAsync();
            return;
            
        }

        public async Task<Tuple<bool, string, List<FavoriteProduct>>> GetFavoriteProducts(HttpContext httpContext)
        {
            var UserId = httpContext.Request.Cookies["UserId"];

            if (UserId == null)
            {
                return new Tuple<bool, string, List<FavoriteProduct>>(false, "User not login", null);
            }

            List<FavoriteProduct> FavPrs = new List<FavoriteProduct>();

            FavPrs = await context.FavoriteProducts
                                .Include(x => x.User)
                                .Include(x => x.Product)
                                .Where(x => x.UserId == int.Parse(UserId)).ToListAsync();
            return new Tuple<bool, string, List<FavoriteProduct>>(true, "OK", FavPrs);
        }

        public async Task<Product> GetProductById(int id)
        {
            return await context.ProductsList.FirstOrDefaultAsync(x=>x.Id==id);
        }

        public async Task<ProductViewModel> GetProductModelById(HttpContext httpContext, string returnURL, int id)
        {
            ProductViewModel model = new ProductViewModel();
            model.Product = await context.ProductsList.Include(x => x.Category).FirstOrDefaultAsync(x => x.Id == id);
            model.ReturnUrl = returnURL;

            return model;
        }

        public async Task<Tuple<bool, string, Order>> GetUserOrder(HttpContext httpContext)
        {
            Order order = null;
            var UserId = httpContext.Request.Cookies["UserId"];
            if (UserId == null)
            {
                return new Tuple<bool, string, Order>(false, "User not login", order);
            }
            order = await context.Orders
                                    .Include(x => x.User)
                                    .Include(x => x.OrderProducts)
                                    .ThenInclude(x => x.Product)
                                    .FirstOrDefaultAsync(x => x.User.Id == int.Parse(UserId));
            if (order == null)
            {
                return new Tuple<bool, string, Order>(false, "Order not found", order);
            }
            int totalSumm = 0;
            foreach (var item in order.OrderProducts)
            {
                totalSumm += item.TotalPrice;
            }
            order.totalPrice = totalSumm;
            context.Orders.Update(order);

            return new Tuple<bool, string, Order>(true, "OK", order);
        }

        public async Task<Tuple<bool, string>> RemoveProductFromCart(HttpContext httpContext, int id)
        {
            var UserId = httpContext.Request.Cookies["UserId"];
            if (UserId == null)
            {
                return new Tuple<bool, string>(false, "User not login");
            }
            Product pr = await context.ProductsList.FirstOrDefaultAsync(x => x.Id == id);
            Order order = await context.Orders
                                .Include(x => x.User)
                                .Include(x => x.OrderProducts)
                                .ThenInclude(x => x.Product)
                                .FirstOrDefaultAsync(x => x.User.Id == int.Parse(UserId));
            if (order != null)
            {
                context.OrderProducts.Remove(
                           await context.OrderProducts.FirstAsync(x => x.ProductId == id && x.OrderId == order.Id)
                           );
                context.Orders.Update(order);
            }


            int totalSumm = 0;
            foreach (var item in order.OrderProducts)
            {
                totalSumm += item.TotalPrice;
            }
            order.totalPrice = totalSumm;
            await context.SaveChangesAsync();
            return new Tuple<bool, string>(true, "OK");
        }

        public async Task<Tuple<bool, string>> RemoveProductFromFavourite(HttpContext httpContext, int id)
        {
            if (httpContext.Request.Cookies["UserId"] == null)
            {
                return new Tuple<bool, string>(false, "User not login");
            }
            FavoriteProduct Favpr = await context.FavoriteProducts.FirstOrDefaultAsync(
                                                    x => x.Id == id);
            if (Favpr != null)
            {
                context.FavoriteProducts.Remove(Favpr);
            }

            await context.SaveChangesAsync();
            return new Tuple<bool, string>(true, "OK");
        }


    }
}
