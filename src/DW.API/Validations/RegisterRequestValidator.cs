using DW.API.Models.Auth;
using FluentValidation;

namespace DW.API.Validations;

public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
{
    public RegisterRequestValidator()
    {
        RuleFor(x => x.Email).EmailAddress();
        RuleFor(x => x.Password)
            .Must(BeAValidPassword)
            .WithMessage("Password must be at least 8 characters long and contain uppercase, lowercase, number and special character");
    }

    private bool BeAValidPassword(string password)
    {
        return password.Length >= 8 &&
               password.Any(char.IsUpper) &&
               password.Any(char.IsLower) &&
               password.Any(char.IsDigit) &&
               password.Any(ch => !char.IsLetterOrDigit(ch));
    }
}