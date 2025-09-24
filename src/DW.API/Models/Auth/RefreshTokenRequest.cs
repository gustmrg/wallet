using System.ComponentModel.DataAnnotations;

namespace DW.API.Models.Auth;

public class RefreshTokenRequest
{
    [Required(ErrorMessage = "Refresh token is required.")]
    public string RefreshToken { get; set; } = string.Empty;
}