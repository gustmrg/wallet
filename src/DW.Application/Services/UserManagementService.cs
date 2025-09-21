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
}