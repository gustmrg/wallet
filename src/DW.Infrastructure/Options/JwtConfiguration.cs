namespace DW.Infrastructure.Options;

public class JwtConfiguration
{
    public string Key { get; set; } = string.Empty;
    public string Issuer { get; set; } = string.Empty;
    public string Audience { get; set; } = string.Empty;
    public int ExpirationInMinutes { get; set; } = 60;
    
    public void Validate()
    {
        if (string.IsNullOrEmpty(Key))
            throw new InvalidOperationException("JWT signing key is required");
            
        if (Key.Length < 32)
            throw new InvalidOperationException("JWT signing key must be at least 32 characters for security");
            
        if (string.IsNullOrEmpty(Issuer))
            throw new InvalidOperationException("JWT Issuer is required");
            
        if (string.IsNullOrEmpty(Audience))
            throw new InvalidOperationException("JWT Audience is required");
            
        if (ExpirationInMinutes <= 0)
            throw new InvalidOperationException("JWT ExpirationInMinutes must be positive");
            
        if (ExpirationInMinutes > 1440)
        {
            throw new InvalidOperationException("JWT ExpirationInMinutes should not exceed 1440 minutes (24 hours) for access tokens");
        }
    }
}