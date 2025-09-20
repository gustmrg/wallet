using DW.Domain.Entities;

namespace DW.Domain.Interfaces;

public interface IRefreshTokenRepository
{
    Task<RefreshToken> CreateAsync(RefreshToken token);
    Task<RefreshToken?> GetByIdAsync(Guid id);
    Task<RefreshToken?> GetByTokenValueAsync(string tokenValue);
    Task<IEnumerable<RefreshToken>> GetActiveTokensByUserIdAsync(Guid userId);
    Task UpdateAsync(RefreshToken token);
    Task DeleteAsync(Guid id);
    Task RevokeAllUserTokensAsync(Guid userId, string reason, string? revokedByIp = null);
}