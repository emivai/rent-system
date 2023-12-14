using FluentValidation;
using RentSystem.Core.DTOs;
using RentSystem.Core.Enums;

namespace RentSystem.Services.Validations
{
    public class UserValidator : AbstractValidator<RegisterUserDTO>
    {
        public UserValidator()
        {
            RuleFor(x => x.Role)
                .NotEqual(Role.Admin).WithMessage("User cannot be admin")
                .IsInEnum().WithMessage("No such role exists");
        }
    }
}
