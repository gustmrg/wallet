using DW.Application.Interfaces;
using DW.Domain.Entities;
using DW.Domain.Interfaces;

namespace DW.Application.Services;

public class UserManagementService : IUserManagementService
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;

    public UserManagementService(IUserRepository userRepository, IPasswordHasher passwordHasher)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
    }

    public async Task RegisterUserAsync(string email, string password)
    {
        var user = new User
        {
            Id = Guid.CreateVersion7(),
            Email = email,
            PasswordHash = _passwordHasher.HashPassword(password),
            CreatedAt = DateTime.UtcNow
        };

        await _userRepository.CreateAsync(user);
        
        // login and return token
    }
}