using System.ComponentModel.DataAnnotations;
using DW.API.Attributes;

namespace DW.API.Models.Auth;

public class LoginRequest
{
    [Required(ErrorMessage = "Email field is required.")]
    [EmailAddress(ErrorMessage = "Invalid email address.")]
    public string Email { get; set; }
    
    [Required(ErrorMessage = "Password field is required.")]
    [StrongPassword(ErrorMessage = "Password does not meet strength requirements.")]
    [DataType(DataType.Password)]
    public string Password { get; set; }
    
    [Required(ErrorMessage = "Confirm Password is required.")]
    [Compare("Password", ErrorMessage = "Password and confirmation password do not match.")]
    [DataType(DataType.Password)]
    public string ConfirmPassword { get; set; }
}