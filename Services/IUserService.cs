using BankOpsPlus.Models;

namespace BankOpsPlus.Services;

/// <summary>
/// Service for user authentication and management
/// </summary>
public interface IUserService
{
    Task<User?> AuthenticateAsync(string email, string password);
    Task<User?> GetUserByIdAsync(int id);
    Task<User?> GetUserByEmailAsync(string email);
    Task<bool> IsInRoleAsync(int userId, UserRole role);
}
