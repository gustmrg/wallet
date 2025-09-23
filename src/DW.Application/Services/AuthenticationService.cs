using DW.Application.Common;
using DW.Application.DTOs.Auth;
using DW.Application.Interfaces;
using DW.Domain.Interfaces;

namespace DW.Application.Services;

public class AuthenticationService : IAuthenticationService
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ITokenService _tokenService;
    private readonly IRefreshTokenService _refreshTokenService;

    public AuthenticationService(
        IUserRepository userRepository, 
        IPasswordHasher passwordHasher, 
        ITokenService tokenService, 
        IRefreshTokenService refreshTokenService)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _tokenService = tokenService;
        _refreshTokenService = refreshTokenService;
    }
    
    public async Task<Result<TokenDto>> AuthenticateUserAsync(string email, string password)
    {
        var user = await _userRepository.GetByEmailAsync(email);

        if (user == null)
            return Result<TokenDto>.Failure(Error.Unauthorized("Invalid email or password"));

        var isValidPassword = _passwordHasher.VerifyPassword(password, user.PasswordHash);

        if (!isValidPassword)
            return Result<TokenDto>.Failure(Error.Unauthorized("Invalid email or password"));

        var tokenResult = await IssueNewTokensAsync(user.Id);
        
        return Result<TokenDto>.Success(tokenResult.Data);
    }

    public async Task<Result<TokenDto>> IssueNewTokensAsync(Guid userId)
    {
        await _refreshTokenService.RevokeAllUserTokensAsync(userId, "Issued new tokens");
        
        var user = await _userRepository.GetByIdAsync(userId);
        var accessToken = _tokenService.GenerateAccessToken(user.Id, user.Email);
        var refreshToken = await _refreshTokenService.GenerateRefreshTokenAsync(user.Id);
        
        return Result<TokenDto>.Success(new TokenDto { AccessToken = accessToken, RefreshToken = refreshToken });
    }

    public async Task<Result<string>> RefreshTokenAsync(Guid userId, string tokenValue)
    {
        await _refreshTokenService.RevokeRefreshTokenAsync(tokenValue, "Refresh token");
        
        var refreshToken = await _refreshTokenService.GenerateRefreshTokenAsync(userId);
        return Result<string>.Success(refreshToken);
    }
}