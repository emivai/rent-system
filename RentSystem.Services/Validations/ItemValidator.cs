using FluentValidation;
using RentSystem.Core.DTOs;
using RentSystem.Core.Enums;

namespace RentSystem.Services.Validations
{
    public class ItemValidator : AbstractValidator<ItemDTO>
    {
        public ItemValidator() 
        {
            RuleFor(x => x.Category)
                .NotEmpty().WithMessage("Category is required")
                .IsInEnum();

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required")
                .Length(1, 50).WithMessage("Length has to be between 1 and 50 characters");

            RuleFor(x => x.Price)
                .NotEmpty().WithMessage("Price is required")
                .GreaterThan(0).WithMessage("Price must be greater than 0");

            RuleFor(x => x.State)
                .IsInEnum();

            RuleFor(x => x.AdvertId)
                .NotEmpty().WithMessage("Item must be assigned to advert");
        }
    }
}
