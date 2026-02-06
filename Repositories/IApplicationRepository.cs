using BankOpsPlus.Models;

namespace BankOpsPlus.Repositories;

/// <summary>
/// Repository interface for Application entity with specialized queries
/// </summary>
public interface IApplicationRepository : IRepository<Application>
{
    /// <summary>
    /// Get all applications by environment
    /// </summary>
    Task<IEnumerable<Application>> GetByEnvironmentAsync(ApplicationEnvironment environment);

    /// <summary>
    /// Get application by unique code
    /// </summary>
    Task<Application?> GetByCodeAsync(string code);

    /// <summary>
    /// Get all active applications (IsActive = true)
    /// </summary>
    Task<IEnumerable<Application>> GetActiveApplicationsAsync();

    /// <summary>
    /// Get applications with their related incidents count
    /// </summary>
    Task<IEnumerable<Application>> GetApplicationsWithIncidentCountAsync();

    /// <summary>
    /// Check if application code already exists (for validation)
    /// </summary>
    Task<bool> CodeExistsAsync(string code, int? excludeId = null);
}
