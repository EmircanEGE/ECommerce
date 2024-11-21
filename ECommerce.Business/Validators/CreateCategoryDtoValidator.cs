using ECommerce.DTOs;
using FluentValidation;

namespace ECommerce.Business.Validators
{
    public class CreateCategoryDtoValidator : AbstractValidator<CreateCategoryDto>
    {
        public CreateCategoryDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Category name is required.")
                .Length(3, 30).WithMessage("Category name must be between 3 and 30 characters.");
        }
    }
}
