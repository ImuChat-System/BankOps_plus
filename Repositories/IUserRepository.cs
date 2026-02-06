using BankOpsPlus.Models;

namespace BankOpsPlus.Repositories;

/// <summary>
/// Repository interface for User-specific operations
/// </summary>
public interface IUserRepository : IRepository<User>
{
    Task<User?> GetByEmailAsync(string email);
    Task<User?> AuthenticateAsync(string email, string password);
    Task UpdateLastLoginAsync(int userId);
}
