using System.ComponentModel.DataAnnotations;

namespace AnimeShop.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public decimal Discount { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public string? ImageURl { get; set; }

        public int CategoryId { get; set; }
        public Category? Category { get; set; }
        public IEnumerable<OrderProducts>? OrderProducts { get; set; }
    }
}
