using DW.Application.Common;
using DW.Application.DTOs.Users;
using DW.Application.Interfaces;
using DW.Domain.Entities;
using DW.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace DW.Application.Services;

public class UserManagementService : IUserManagementService
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ILogger<UserManagementService> _logger;

    public UserManagementService(
        IUserRepository userRepository, 
        IPasswordHasher passwordHasher, ILogger<UserManagementService> logger)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _logger = logger;
    }

    public async Task<Result<UserDto>> RegisterUserAsync(string email, string password)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(email))
                return Result<UserDto>.Failure("Email is required", ErrorType.Validation);

            if (string.IsNullOrWhiteSpace(password))
                return Result<UserDto>.Failure("Password is required", ErrorType.Validation);

            if (!IsValidEmail(email))
                return Result<UserDto>.Failure("Invalid email format", ErrorType.Validation);

            if (!IsValidPassword(password))
                return Result<UserDto>.Failure("Password must be at least 8 characters long and contain uppercase, lowercase, number and special character", ErrorType.Validation);

            var existingUser = await _userRepository.GetByEmailAsync(email);
            if (existingUser != null)
                return Result<UserDto>.Failure("User with this email already exists", ErrorType.Conflict);

            var user = new User
            {
                Id = Guid.CreateVersion7(),
                Email = email.ToLowerInvariant(),
                PasswordHash = _passwordHasher.HashPassword(password),
                CreatedAt = DateTime.UtcNow
            };

            await _userRepository.CreateAsync(user);

            _logger.LogInformation("User registered successfully with email {Email} and ID {UserId}", email, user.Id);

            var data = new UserDto { Id = user.Id, Email = user.Email };
            return Result<UserDto>.Success(data);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to register user with email {Email}: {ErrorMessage}", email, ex.Message);
            return Result<UserDto>.Failure("An error occurred while creating the user account", ErrorType.Internal);
        }
    }

    private static bool IsValidEmail(string email)
    {
        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == email;
        }
        catch
        {
            return false;
        }
    }

    private static bool IsValidPassword(string password)
    {
        return password.Length >= 8 &&
               password.Any(char.IsUpper) &&
               password.Any(char.IsLower) &&
               password.Any(char.IsDigit) &&
               password.Any(ch => !char.IsLetterOrDigit(ch));
    }
}