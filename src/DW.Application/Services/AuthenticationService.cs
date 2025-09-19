using DW.Application.Interfaces;
using DW.Domain.Interfaces;

namespace DW.Application.Services;

public class AuthenticationService : IAuthenticationService
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ITokenService _tokenService;

    public AuthenticationService(IUserRepository userRepository, IPasswordHasher passwordHasher, ITokenService tokenService)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _tokenService = tokenService;
    }
    
    public async Task AuthenticateUserAsync(string email, string password)
    {
        var user = await _userRepository.GetByEmailAsync(email);

        if (user == null)
            return;

        var isValidPassword = _passwordHasher.VerifyPassword(password, user.PasswordHash);

        if (!isValidPassword)
            return;

        var token = _tokenService.GenerateAccessToken(user.Id, user.Email);
    }
}