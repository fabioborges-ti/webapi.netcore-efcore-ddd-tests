using Api.Domain.Dtos.User;
using Api.Domain.Validations.ValidationRoles;
using FluentValidation;
using System.Diagnostics.CodeAnalysis;

namespace Api.Domain.Validations.Dtos.User
{
    [ExcludeFromCodeCoverage]
    public class UserDtoCreateValidator : AbstractValidator<UserDtoCreate>
    {
        public UserDtoCreateValidator()
        {
            RuleFor(u => u.Name)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .Length(3, 100).WithMessage("{PropertyName} must contain between {MinLength} and {MaxLength} characters.")
                .Must(Roles.IsValidName).WithMessage("{PropertyName} contains invalid characters.");

            RuleFor(u => u.Email)
                .Cascade(CascadeMode.Stop)    
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .EmailAddress().WithMessage("{PropertyName} invalid format.");

            RuleFor(u => u.Document)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("{PropertyName} is required")
                .Length(14).WithMessage("{PropertyName} must contain {MinLength} characters.")
                .Must(Roles.IsValidDocument).WithMessage("{PropertyName} invalid document.");
        }
    }
}
