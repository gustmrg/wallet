using DW.Domain.Entities;
using DW.Domain.Interfaces;

namespace DW.Infrastructure.Repositories;

public class RefreshTokenRepository : IRefreshTokenRepository
{
    public Task<RefreshToken> CreateAsync(RefreshToken token)
    {
        throw new NotImplementedException();
    }

    public Task<RefreshToken?> GetByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task UpdateAsync(RefreshToken token)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(Guid id)
    {
        throw new NotImplementedException();
    }
}