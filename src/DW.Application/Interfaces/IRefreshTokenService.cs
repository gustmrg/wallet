using DW.Application.Common;

namespace DW.Application.Interfaces;

/// <summary>
/// Manages refresh tokens for maintaining user sessions.
/// This service handles the stateful aspects of authentication.
/// </summary>
public interface IRefreshTokenService
{
    /// <summary>
    /// Creates a new refresh token for a user session.
    /// The token will be stored securely and can be revoked.
    /// </summary>
    Task<string> GenerateRefreshTokenAsync(Guid userId);
    
    /// <summary>
    /// Validates a refresh token and marks it as used.
    /// Returns null if the token is invalid, expired, or revoked.
    /// </summary>
    Task<Result> ValidateRefreshTokenAsync(string tokenValue);
    
    /// <summary>
    /// Revokes a specific refresh token, ending that session.
    /// </summary>
    Task RevokeRefreshTokenAsync(string tokenValue, string reason);
    
    /// <summary>
    /// Revokes all refresh tokens for a user, ending all sessions.
    /// Useful for security incidents or when user changes password.
    /// </summary>
    Task RevokeAllUserTokensAsync(Guid userId, string reason);

    /// <summary>
    /// Gets the user ID associated with a valid refresh token.
    /// Returns null if token is invalid, expired, or revoked.
    /// </summary>
    Task<Guid?> GetUserIdFromTokenAsync(string tokenValue);
}