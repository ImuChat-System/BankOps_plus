using BankOpsPlus.Models;

namespace BankOpsPlus.Services;

/// <summary>
/// Service interface for Application business logic
/// </summary>
public interface IApplicationService
{
    /// <summary>
    /// Get all applications
    /// </summary>
    Task<IEnumerable<Application>> GetAllApplicationsAsync();

    /// <summary>
    /// Get applications filtered by environment
    /// </summary>
    Task<IEnumerable<Application>> GetApplicationsByEnvironmentAsync(ApplicationEnvironment? environment);

    /// <summary>
    /// Get application by ID
    /// </summary>
    Task<Application?> GetApplicationByIdAsync(int id);

    /// <summary>
    /// Get all active applications
    /// </summary>
    Task<IEnumerable<Application>> GetActiveApplicationsAsync();

    /// <summary>
    /// Create new application with validation
    /// </summary>
    Task<Application> CreateApplicationAsync(Application application);

    /// <summary>
    /// Update existing application with validation
    /// </summary>
    Task UpdateApplicationAsync(Application application);

    /// <summary>
    /// Soft delete application (check for related incidents)
    /// </summary>
    Task<bool> DeleteApplicationAsync(int id);

    /// <summary>
    /// Validate application code uniqueness
    /// </summary>
    Task<bool> IsCodeUniqueAsync(string code, int? excludeId = null);
}
