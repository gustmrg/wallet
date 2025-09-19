namespace DW.Application.Interfaces;

public interface IAuthenticationService
{
    Task AuthenticateUserAsync(string email, string password);
}