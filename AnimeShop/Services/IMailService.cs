using AnimeShop.Models;

namespace AnimeShop.Services
{
    public interface IMailService
    {
        Task SendAsync(Order order, string subject);
        Task SendAsync(User user, string subject);
    }
}
