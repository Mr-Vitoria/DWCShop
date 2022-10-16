using System.ComponentModel.DataAnnotations;

namespace AnimeShop.Models
{
    public class Category
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        public IEnumerable<Product>? Products { get; set; }
        public string? ImageUrl { get; set; }
    }
}
