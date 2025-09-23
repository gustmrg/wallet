using DW.Application.Common;
using DW.Application.DTOs.Auth;

namespace DW.Application.Interfaces;

public interface IAuthenticationService
{
    Task<Result<TokenDto>> AuthenticateUserAsync(string email, string password);
    Task<Result<TokenDto>> IssueNewTokensAsync(Guid userId);
    Task<Result<string>> RefreshTokenAsync(Guid userId, string tokenValue);
}