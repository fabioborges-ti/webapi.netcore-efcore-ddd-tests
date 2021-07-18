using Api.Domain.Dtos.User;
using Api.Domain.Validations.ValidationRoles;
using FluentValidation;
using System.Diagnostics.CodeAnalysis;

namespace Api.Domain.Validations.Dtos.User
{
    [ExcludeFromCodeCoverage]
    public class UserDtoUpdateValidator : AbstractValidator<UserDtoUpdate>
    {
        public UserDtoUpdateValidator()
        {
            RuleFor(u => u.Id)
                .Cascade(CascadeMode.Stop)
                .NotNull().WithMessage("{PropertyName} is required.")
                .NotEmpty().WithMessage("{PropertyName} is required.");

            RuleFor(u => u.Name)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .Length(3, 100).WithMessage("{PropertyName} must contain between {MinLength} and {MaxLength} characters.")
                .Must(Roles.IsValidName).WithMessage("{PropertyName} contains invalid characters.");

            RuleFor(u => u.Email)
                .Cascade(CascadeMode.Stop)    
                .NotEmpty().WithMessage("{PropertyName} - is required.")
                .EmailAddress().WithMessage("{PropertyName} invalid format.");

            RuleFor(u => u.Document)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("{PropertyName} is required")
                .Length(14).WithMessage("{PropertyName} must contain {MinLength} characters.")
                .Must(Roles.IsValidDocument).WithMessage("{PropertyName} invalid document.");
        }
    }
}
