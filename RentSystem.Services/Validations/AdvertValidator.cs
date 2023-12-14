using FluentValidation;
using RentSystem.Core.DTOs;

namespace RentSystem.Services.Validations
{
    internal class AdvertValidator : AbstractValidator<AdvertDTO>
    {
        public AdvertValidator() 
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required")
                .Length(1, 50).WithMessage("Title length has to be between 1 and 50 characters");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Description is required")
                .Length(1, 200).WithMessage("Description length has to be between 1 and 200 characters"); ;

            RuleFor(x => x.RentStart.Date)
                .NotEmpty().WithMessage("Rent start date is required")
                .GreaterThanOrEqualTo(DateTime.Now.Date).WithMessage("Start date cannot be in the past")
                .LessThan(x => x.RentEnd.Date).WithMessage("Start date has to be before end date");

            RuleFor(x => x.RentEnd.Date)
                .NotEmpty().WithMessage("Rent end date is required")
                .GreaterThanOrEqualTo(DateTime.Now.Date)
                .WithMessage("End date cannot be in the past").GreaterThan(x => x.RentStart.Date).WithMessage("End date has to be after start date");

            RuleFor(x => x.DeliveryType)
                .IsInEnum().WithMessage("No such delivery type exists");
        }
    }
}
