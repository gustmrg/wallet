using System.Security.Cryptography;
using DW.Application.Interfaces;
using DW.Domain.Entities;
using DW.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace DW.Infrastructure.Services;

public class RefreshTokenService : IRefreshTokenService
{
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly ILogger<RefreshTokenService> _logger;
    private const int RefreshTokenLifetimeDays = 7;

    public RefreshTokenService(
        IRefreshTokenRepository refreshTokenRepository,
        ILogger<RefreshTokenService> logger)
    {
        _refreshTokenRepository = refreshTokenRepository;
        _logger = logger;
    }

    public async Task<string> GenerateRefreshTokenAsync(Guid userId)
    {
        try
        {
            var tokenValue = GenerateSecureToken();

            var refreshToken = new RefreshToken
            {
                Id = Guid.CreateVersion7(),
                TokenValue = tokenValue,
                UserId = userId,
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddDays(RefreshTokenLifetimeDays)
            };

            await _refreshTokenRepository.CreateAsync(refreshToken);

            _logger.LogInformation("Refresh token generated for user {UserId}", userId);

            return tokenValue;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to generate refresh token for user {UserId}", userId);
            throw;
        }
    }

    public async Task<string?> ValidateRefreshTokenAsync(string tokenValue)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(tokenValue))
                return null;

            var refreshToken = await _refreshTokenRepository.GetByTokenValueAsync(tokenValue);

            if (refreshToken == null || !refreshToken.IsActive)
            {
                _logger.LogWarning("Invalid or inactive refresh token attempted: {TokenPrefix}",
                    tokenValue[..Math.Min(8, tokenValue.Length)]);
                return null;
            }

            refreshToken.UsedAt = DateTime.UtcNow;
            await _refreshTokenRepository.UpdateAsync(refreshToken);

            _logger.LogInformation("Refresh token validated and marked as used for user {UserId}",
                refreshToken.UserId);

            return refreshToken.UserId.ToString();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error validating refresh token");
            return null;
        }
    }

    public async Task RevokeRefreshTokenAsync(string tokenValue, string reason)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(tokenValue))
                return;

            var refreshToken = await _refreshTokenRepository.GetByTokenValueAsync(tokenValue);

            if (refreshToken == null || refreshToken.IsRevoked)
                return;

            refreshToken.RevokedAt = DateTime.UtcNow;
            refreshToken.RevokedReason = reason;

            await _refreshTokenRepository.UpdateAsync(refreshToken);

            _logger.LogInformation("Refresh token revoked for user {UserId}. Reason: {Reason}",
                refreshToken.UserId, reason);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to revoke refresh token. Reason: {Reason}", reason);
            throw;
        }
    }

    public async Task RevokeAllUserTokensAsync(Guid userId, string reason)
    {
        try
        {
            await _refreshTokenRepository.RevokeAllUserTokensAsync(userId, reason);

            _logger.LogInformation("All refresh tokens revoked for user {UserId}. Reason: {Reason}",
                userId, reason);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to revoke all tokens for user {UserId}. Reason: {Reason}",
                userId, reason);
            throw;
        }
    }

    private static string GenerateSecureToken()
    {
        var randomBytes = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomBytes);
        return Convert.ToBase64String(randomBytes);
    }
}