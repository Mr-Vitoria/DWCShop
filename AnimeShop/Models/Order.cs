namespace AnimeShop.Models
{
    public class Order
    {
        public int Id { get; set; }
        public User User { get; set; }
        public IEnumerable<OrderProducts> OrderProducts { get; set; }
        public int totalPrice { get; set; }
    }
}
