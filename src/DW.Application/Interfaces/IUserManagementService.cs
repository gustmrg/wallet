namespace DW.Application.Interfaces;

public interface IUserManagementService
{
    Task RegisterUserAsync(string email, string password);
}