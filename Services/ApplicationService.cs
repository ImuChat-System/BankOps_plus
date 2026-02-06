using BankOpsPlus.Models;
using BankOpsPlus.Repositories;

namespace BankOpsPlus.Services;

/// <summary>
/// Service implementation for Application business logic
/// </summary>
public class ApplicationService : IApplicationService
{
    private readonly IApplicationRepository _applicationRepository;

    public ApplicationService(IApplicationRepository applicationRepository)
    {
        _applicationRepository = applicationRepository;
    }

    /// <summary>
    /// Get all applications
    /// </summary>
    public async Task<IEnumerable<Application>> GetAllApplicationsAsync()
    {
        return await _applicationRepository.GetApplicationsWithIncidentCountAsync();
    }

    /// <summary>
    /// Get applications filtered by environment
    /// </summary>
    public async Task<IEnumerable<Application>> GetApplicationsByEnvironmentAsync(ApplicationEnvironment? environment)
    {
        if (environment.HasValue)
        {
            return await _applicationRepository.GetByEnvironmentAsync(environment.Value);
        }

        return await GetAllApplicationsAsync();
    }

    /// <summary>
    /// Get application by ID
    /// </summary>
    public async Task<Application?> GetApplicationByIdAsync(int id)
    {
        return await _applicationRepository.GetByIdAsync(id);
    }

    /// <summary>
    /// Get all active applications
    /// </summary>
    public async Task<IEnumerable<Application>> GetActiveApplicationsAsync()
    {
        return await _applicationRepository.GetActiveApplicationsAsync();
    }

    /// <summary>
    /// Create new application with validation
    /// </summary>
    public async Task<Application> CreateApplicationAsync(Application application)
    {
        // Validate code uniqueness
        if (await _applicationRepository.CodeExistsAsync(application.Code))
        {
            throw new InvalidOperationException($"Le code d'application '{application.Code}' existe déjà.");
        }

        // Set defaults
        application.IsActive = true;
        application.CreatedAt = DateTime.UtcNow;

        await _applicationRepository.AddAsync(application);
        return application;
    }

    /// <summary>
    /// Update existing application with validation
    /// </summary>
    public async Task UpdateApplicationAsync(Application application)
    {
        var existing = await _applicationRepository.GetByIdAsync(application.Id);
        if (existing == null)
        {
            throw new ArgumentException($"Application avec ID {application.Id} introuvable.");
        }

        // Validate code uniqueness (excluding current application)
        if (await _applicationRepository.CodeExistsAsync(application.Code, application.Id))
        {
            throw new InvalidOperationException($"Le code d'application '{application.Code}' existe déjà.");
        }

        // Preserve CreatedAt
        application.CreatedAt = existing.CreatedAt;

        await _applicationRepository.UpdateAsync(application);
    }

    /// <summary>
    /// Soft delete application (set IsActive = false)
    /// Check if application has related incidents
    /// </summary>
    public async Task<bool> DeleteApplicationAsync(int id)
    {
        var application = await _applicationRepository.GetByIdAsync(id);
        if (application == null)
        {
            return false;
        }

        // Check for related incidents
        var appWithIncidents = await _applicationRepository.GetApplicationsWithIncidentCountAsync();
        var targetApp = appWithIncidents.FirstOrDefault(a => a.Id == id);

        if (targetApp?.Incidents?.Any() == true)
        {
            throw new InvalidOperationException(
                $"Impossible de supprimer l'application '{application.Name}' car elle a {targetApp.Incidents.Count} incident(s) associé(s).");
        }

        // Soft delete
        application.IsActive = false;
        await _applicationRepository.UpdateAsync(application);

        return true;
    }

    /// <summary>
    /// Validate application code uniqueness
    /// </summary>
    public async Task<bool> IsCodeUniqueAsync(string code, int? excludeId = null)
    {
        return !await _applicationRepository.CodeExistsAsync(code, excludeId);
    }
}
