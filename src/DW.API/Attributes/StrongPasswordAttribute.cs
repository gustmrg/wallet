using System.ComponentModel.DataAnnotations;

namespace DW.API.Attributes;

public class StrongPasswordAttribute : ValidationAttribute
{
    public int MinimumLength { get; set; } = 8;
    public bool RequireUppercase { get; set; } = true;
    public bool RequireLowercase { get; set; } = true;
    public bool RequireDigit { get; set; } = true;
    public bool RequireSpecialCharacter { get; set; } = true;

    public override bool IsValid(object? value)
    {
        if (value is not string password)
            return false;

        if (string.IsNullOrWhiteSpace(password))
            return false;

        if (password.Length < MinimumLength)
            return false;

        if (RequireUppercase && !password.Any(char.IsUpper))
            return false;

        if (RequireLowercase && !password.Any(char.IsLower))
            return false;

        if (RequireDigit && !password.Any(char.IsDigit))
            return false;

        if (RequireSpecialCharacter && !password.Any(c => !char.IsLetterOrDigit(c)))
            return false;

        return true;
    }

    public override string FormatErrorMessage(string name)
    {
        var requirements = new List<string>();

        requirements.Add($"at least {MinimumLength} characters");

        if (RequireUppercase)
            requirements.Add("an uppercase letter");

        if (RequireLowercase)
            requirements.Add("a lowercase letter");

        if (RequireDigit)
            requirements.Add("a digit");

        if (RequireSpecialCharacter)
            requirements.Add("a special character");

        return $"{name} must contain {string.Join(", ", requirements)}.";
    }
}