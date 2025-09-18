using DW.Application.Interfaces;
using DW.Domain.Entities;

namespace DW.Infrastructure.Services;

public class JwtTokenService : ITokenService
{
    public Task<string> GenerateAccessTokenAsync(User user)
    {
        throw new NotImplementedException();
    }
}