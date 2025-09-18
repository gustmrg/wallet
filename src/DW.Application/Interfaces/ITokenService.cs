using DW.Domain.Entities;

namespace DW.Application.Interfaces;

/// <summary>
/// Handles the generation and validation of JWT access tokens.
/// This is an application service that supports authentication use cases.
/// </summary>
public interface ITokenService
{
    /// <summary>
    /// Generates a new JWT access token containing user claims.
    /// The token will be self-validating and stateless.
    /// </summary>
    Task<string> GenerateAccessTokenAsync(User user);
}