namespace DW.API.Models.Auth;

public class RegisterResponse
{
    public Guid UserId { get; set; }
    public string Email { get; set; }
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
}