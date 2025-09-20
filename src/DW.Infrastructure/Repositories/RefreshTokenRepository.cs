using DW.Domain.Entities;
using DW.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DW.Infrastructure.Repositories;

public class RefreshTokenRepository : IRefreshTokenRepository
{
    private readonly ApplicationDbContext _context;

    public RefreshTokenRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<RefreshToken> CreateAsync(RefreshToken token)
    {
        await _context.RefreshTokens.AddAsync(token);
        await _context.SaveChangesAsync();
        return token;
    }

    public Task<RefreshToken?> GetByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public async Task<RefreshToken?> GetByTokenValueAsync(string tokenValue)
    {
        return await _context.RefreshTokens.FirstOrDefaultAsync(x => x.TokenValue == tokenValue);
    }

    public async Task<IEnumerable<RefreshToken>> GetActiveTokensByUserIdAsync(Guid userId)
    {
        return await _context.RefreshTokens
            .Where(x => x.UserId == userId && x.IsActive)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task UpdateAsync(RefreshToken token)
    {
        _context.RefreshTokens.Update(token);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var token = await _context.RefreshTokens.FindAsync(id);

        if (token != null)
        {
            _context.RefreshTokens.Remove(token);
            await _context.SaveChangesAsync();    
        }
    }

    public Task RevokeAllUserTokensAsync(Guid userId, string reason, string? revokedByIp = null)
    {
        throw new NotImplementedException();
    }
}