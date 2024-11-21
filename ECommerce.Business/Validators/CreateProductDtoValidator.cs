using ECommerce.DTOs;
using FluentValidation;

namespace ECommerce.Business.Validators
{
    public class CreateProductDtoValidator : AbstractValidator<CreateProductDto>
    {
        public CreateProductDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Product name is required.")
                .Length(3, 50).WithMessage("Product name must be between 3 and 50 characters.");
            RuleFor(x => x.Price)
                .GreaterThan(0).WithMessage("Price must be greater than 0.");
            RuleFor(x => x.Stock)
                .GreaterThan(-1).WithMessage("Stock must be positive number.");
            RuleFor(x => x.CategoryId)
                .GreaterThan(0).WithMessage("Category id must be valid");
        }
    }
}
