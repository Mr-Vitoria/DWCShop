using System.ComponentModel.DataAnnotations;

namespace AnimeShop.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string FirstName { get; set; }
        public string SurName { get; set; }
        [Phone]
        public string Phone { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }
        public string TypeOfAcc { get; set; }
        public string Email { get; set; }
        public string? ImageURl { get; set; }
    }
}
