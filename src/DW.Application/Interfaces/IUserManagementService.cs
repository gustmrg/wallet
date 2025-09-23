using DW.Application.Common;
using DW.Application.DTOs.Users;

namespace DW.Application.Interfaces;

public interface IUserManagementService
{
    Task<Result<UserDto>> RegisterUserAsync(string email, string password);
    Task<Result<UserDto>> GetUserAsync(string email);
}