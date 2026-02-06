using BankOpsPlus.Data;
using BankOpsPlus.Models;
using Microsoft.EntityFrameworkCore;

namespace BankOpsPlus.Repositories;

/// <summary>
/// Repository implementation for User authentication and management
/// </summary>
public class UserRepository : Repository<User>, IUserRepository
{
    public UserRepository(BankOpsDbContext context) : base(context)
    {
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        return await _dbSet
            .FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<User?> AuthenticateAsync(string email, string password)
    {
        var user = await GetByEmailAsync(email);

        if (user == null || !user.IsActive)
        {
            return null;
        }

        // Verify password using BCrypt
        bool isValidPassword = BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);

        return isValidPassword ? user : null;
    }

    public async Task UpdateLastLoginAsync(int userId)
    {
        var user = await GetByIdAsync(userId);
        if (user != null)
        {
            user.LastLoginAt = DateTime.UtcNow;
            await UpdateAsync(user);
        }
    }
}
