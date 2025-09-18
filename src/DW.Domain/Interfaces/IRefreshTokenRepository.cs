using DW.Domain.Entities;

namespace DW.Domain.Interfaces;

public interface IRefreshTokenRepository
{
    Task<RefreshToken> CreateAsync(RefreshToken token);
    Task<RefreshToken?> GetByIdAsync(Guid id);
    Task UpdateAsync(RefreshToken token);
    Task DeleteAsync(Guid id);
}