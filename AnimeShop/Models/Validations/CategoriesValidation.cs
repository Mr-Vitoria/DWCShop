using FluentValidation;

namespace AnimeShop.Models.Validations
{
    public class CategoriesValidation:AbstractValidator<Category>
    {
        public CategoriesValidation()
        {

            RuleFor(x => x.Title).Length(0, 25);
            RuleFor(x => x.Title).NotNull();
        }
    }
}
