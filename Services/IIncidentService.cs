using BankOpsPlus.Models;

namespace BankOpsPlus.Services;

/// <summary>
/// Service for incident lifecycle management
/// </summary>
public interface IIncidentService
{
    Task<IEnumerable<Incident>> GetAllIncidentsAsync();
    Task<IEnumerable<Incident>> GetIncidentsByFiltersAsync(IncidentStatus? status, IncidentSeverity? severity);
    Task<Incident?> GetIncidentByIdAsync(int id);
    Task<Incident> CreateIncidentAsync(Incident incident, int createdByUserId);
    Task<Incident> UpdateIncidentAsync(Incident incident);
    Task<Incident> ResolveIncidentAsync(int id, string rootCause);
    Task DeleteIncidentAsync(int id);
}
