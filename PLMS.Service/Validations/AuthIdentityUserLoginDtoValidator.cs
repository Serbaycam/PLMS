using PLMS.Core.DTOs;
using FluentValidation;

namespace PLMS.Service.Validations
{
    public class AuthIdentityUserLoginDtoValidator : AbstractValidator<AuthIdentityUserLoginDto>
    {
        public AuthIdentityUserLoginDtoValidator()
        {

            RuleFor(x => x.Email).NotNull().WithMessage("{PropertyName} is required.").NotEmpty().WithMessage("{PropertyName} cannot be empty");
            RuleFor(x => x.Password).NotNull().WithMessage("{PropertyName} is required.").NotEmpty().WithMessage("{PropertyName} cannot be empty");
        }
    }
}
