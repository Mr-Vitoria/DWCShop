using System.Collections.Concurrent;

namespace AnimeShop.Models
{
    public class ShopViewModel
    {
        public List<Category> Categories { get; set; }
        public List<Product> Products { get; set; }
        public List<Product> FavoriteProducts { get; set; }
        public ConcurrentQueue<Product> RecentProducts { get; set; }
        public User User { get; set; }
        public static int CountItemCart { get; set; } = 0;
        public static int CountItemFacourites { get; set; } = 0;

        public string SearchTitle { get; set; }
        public int CategoryID { get; set; }
        public int? minPriceProd { get; set; }
        public int? maxPriceProd { get; set; }
        public int minPrice { get; set; }
        public int maxPrice { get; set; }

        public string Status { get; set; }
    }
}
