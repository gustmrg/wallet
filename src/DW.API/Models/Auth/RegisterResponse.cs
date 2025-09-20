namespace DW.API.Models.Auth;

public class RegisterResponse
{
    public bool IsSuccess { get; set; }
    public string? Message { get; set; }
    public Guid UserId { get; set; }
    public string Email { get; set; }
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
}