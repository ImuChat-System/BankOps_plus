using BankOpsPlus.Data;
using BankOpsPlus.Models;
using Microsoft.EntityFrameworkCore;

namespace BankOpsPlus.Repositories;

/// <summary>
/// Repository implementation for Application entity
/// </summary>
public class ApplicationRepository : Repository<Application>, IApplicationRepository
{
    public ApplicationRepository(BankOpsDbContext context) : base(context)
    {
    }

    /// <summary>
    /// Get all applications by environment
    /// </summary>
    public async Task<IEnumerable<Application>> GetByEnvironmentAsync(ApplicationEnvironment environment)
    {
        return await _dbSet
            .Where(a => a.Environment == environment)
            .OrderBy(a => a.Name)
            .ToListAsync();
    }

    /// <summary>
    /// Get application by unique code
    /// </summary>
    public async Task<Application?> GetByCodeAsync(string code)
    {
        return await _dbSet
            .FirstOrDefaultAsync(a => a.Code == code);
    }

    /// <summary>
    /// Get all active applications
    /// </summary>
    public async Task<IEnumerable<Application>> GetActiveApplicationsAsync()
    {
        return await _dbSet
            .Where(a => a.IsActive)
            .OrderBy(a => a.Name)
            .ToListAsync();
    }

    /// <summary>
    /// Get applications with their related incidents count
    /// </summary>
    public async Task<IEnumerable<Application>> GetApplicationsWithIncidentCountAsync()
    {
        return await _dbSet
            .Include(a => a.Incidents)
            .OrderBy(a => a.Name)
            .ToListAsync();
    }

    /// <summary>
    /// Check if application code already exists
    /// </summary>
    /// <param name="code">Application code to check</param>
    /// <param name="excludeId">Application ID to exclude from check (for updates)</param>
    public async Task<bool> CodeExistsAsync(string code, int? excludeId = null)
    {
        var query = _dbSet.Where(a => a.Code == code);

        if (excludeId.HasValue)
        {
            query = query.Where(a => a.Id != excludeId.Value);
        }

        return await query.AnyAsync();
    }
}
