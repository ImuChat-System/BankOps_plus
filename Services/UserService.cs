using BankOpsPlus.Models;
using BankOpsPlus.Repositories;

namespace BankOpsPlus.Services;

/// <summary>
/// Service implementation for user authentication and authorization
/// </summary>
public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<User?> AuthenticateAsync(string email, string password)
    {
        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
        {
            return null;
        }

        var user = await _userRepository.AuthenticateAsync(email, password);

        if (user != null)
        {
            // Update last login timestamp
            await _userRepository.UpdateLastLoginAsync(user.Id);
        }

        return user;
    }

    public async Task<User?> GetUserByIdAsync(int id)
    {
        return await _userRepository.GetByIdAsync(id);
    }

    public async Task<User?> GetUserByEmailAsync(string email)
    {
        return await _userRepository.GetByEmailAsync(email);
    }

    public async Task<bool> IsInRoleAsync(int userId, UserRole role)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        return user != null && user.Role == role && user.IsActive;
    }
}
