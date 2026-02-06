using BankOpsPlus.Models;

namespace BankOpsPlus.Repositories;

/// <summary>
/// Repository interface for Incident-specific operations
/// </summary>
public interface IIncidentRepository : IRepository<Incident>
{
    Task<IEnumerable<Incident>> GetIncidentsWithDetailsAsync();
    Task<IEnumerable<Incident>> GetOpenIncidentsAsync();
    Task<IEnumerable<Incident>> GetCriticalIncidentsAsync();
    Task<IEnumerable<Incident>> GetIncidentsByStatusAsync(IncidentStatus status);
    Task<IEnumerable<Incident>> GetIncidentsBySeverityAsync(IncidentSeverity severity);
    Task<IEnumerable<Incident>> GetRecentIncidentsAsync(int count = 10);
    Task<double> CalculateMTTRAsync();
    Task<string> GenerateIncidentReferenceAsync();
}
