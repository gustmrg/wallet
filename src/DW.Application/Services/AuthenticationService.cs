using DW.Application.Interfaces;

namespace DW.Application.Services;

public class AuthenticationService : IAuthenticationService
{
    public Task AuthenticateUserAsync(string email, string password)
    {
        throw new NotImplementedException();
    }
}