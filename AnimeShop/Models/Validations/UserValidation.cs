using FluentValidation;

namespace AnimeShop.Models.Validations
{
	public class UserValidation: AbstractValidator<User>
    {
        public UserValidation()
        {
            RuleFor(x => x.Login).Length(4, 25);
            RuleFor(x => x.Login).NotNull();
            RuleFor(x => x.FirstName).Length(1, 25);
            RuleFor(x => x.FirstName).NotNull();
            RuleFor(x => x.SurName).Length(1, 25);
            RuleFor(x => x.SurName).NotNull();
            RuleFor(x => x.Phone).NotNull();
            RuleFor(x => x.Password).Length(6, 25);
            RuleFor(x => x.Password).NotNull();
        }
    }
}
