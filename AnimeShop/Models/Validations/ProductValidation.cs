using FluentValidation;

namespace AnimeShop.Models.Validations
{
    public class ProductValidation:AbstractValidator<Product>
    {
        public ProductValidation()
        {
            RuleFor(x => x.Title).Length(0, 50);
            RuleFor(x => x.Title).NotNull();

            RuleFor(x => x.Price).NotNull();
            RuleFor(x => x.Discount).Null();
            RuleFor(x => x.Discount).LessThanOrEqualTo(100);
            RuleFor(x => x.Discount).GreaterThanOrEqualTo(0);

            RuleFor(x => x.Description).Length(20, 300);
            RuleFor(x => x.Description).NotNull();
        }
    }
}
