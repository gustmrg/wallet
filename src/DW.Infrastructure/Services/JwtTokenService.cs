using System.Security.Claims;
using System.Text;
using DW.Application.Interfaces;
using DW.Infrastructure.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace DW.Infrastructure.Services;

public class JwtTokenService : ITokenService
{
    private readonly JwtConfiguration _jwtConfig;
    private readonly ILogger<JwtTokenService> _logger;

    public JwtTokenService(IOptions<JwtConfiguration> jwtConfig, ILogger<JwtTokenService> logger)
    {
        _jwtConfig = jwtConfig.Value;
        _logger = logger;
        
        _jwtConfig.Validate();
    }

    public string GenerateAccessTokenAsync(string userId, string email)
    {
        try
        {
            if (string.IsNullOrEmpty(userId))
                throw new ArgumentException("User ID is required", nameof(userId));

            if (string.IsNullOrEmpty(email))
                throw new ArgumentException("Email is required", nameof(email));

            var issueTime = DateTime.UtcNow;
            var expirationTime = issueTime.AddMinutes(_jwtConfig.ExpirationInMinutes);

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtConfig.Key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = BuildClaims(userId, email, issueTime);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = expirationTime,
                Issuer = _jwtConfig.Issuer,
                Audience = _jwtConfig.Audience,
                SigningCredentials = credentials
            };

            var tokenHandler = new JsonWebTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return token;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to generate JWT token for user {UserId}", userId);
            throw new InvalidOperationException("Failed to generate authentication token", ex);
        }
    }

    private static List<Claim> BuildClaims(string userId, string email, DateTime issueTime)
    {
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, userId),
            new(JwtRegisteredClaimNames.Email, email),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(JwtRegisteredClaimNames.Iat, 
                ((DateTimeOffset)issueTime).ToUnixTimeSeconds().ToString(), 
                ClaimValueTypes.Integer64)
        };

        return claims;
    }
}