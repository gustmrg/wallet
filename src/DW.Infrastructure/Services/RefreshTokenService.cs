using DW.Application.Interfaces;

namespace DW.Infrastructure.Services;

public class RefreshTokenService : IRefreshTokenService
{
    public Task<string> GenerateRefreshTokenAsync(Guid userId)
    {
        throw new NotImplementedException();
    }

    public Task<string?> ValidateRefreshTokenAsync(string tokenValue)
    {
        throw new NotImplementedException();
    }

    public Task RevokeRefreshTokenAsync(string tokenValue, string reason)
    {
        throw new NotImplementedException();
    }

    public Task RevokeAllUserTokensAsync(string userId, string reason)
    {
        throw new NotImplementedException();
    }
}