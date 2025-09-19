namespace DW.Application.Interfaces;

/// <summary>
/// Provides functionality for password hashing and verification.
/// </summary>
public interface IPasswordHasher
{
    /// <summary>
    /// Hashes a plain text password using a secure hashing algorithm with salt.
    /// </summary>
    /// <param name="password">The plain text password to hash.</param>
    /// <returns>A base64-encoded string containing the salt and hash.</returns>
    string HashPassword(string password);

    /// <summary>
    /// Verifies a plain text password against a previously hashed password.
    /// </summary>
    /// <param name="password">The plain text password to verify.</param>
    /// <param name="hashedPassword">The base64-encoded hashed password to compare against.</param>
    /// <returns>True if the password matches the hash; otherwise, false.</returns>
    bool VerifyPassword(string password, string hashedPassword);
}