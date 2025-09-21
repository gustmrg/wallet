using System.ComponentModel.DataAnnotations;

namespace DW.API.Models.Auth;

public class RegisterRequest
{
    public string Email { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;
}