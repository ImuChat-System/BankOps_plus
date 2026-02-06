using BankOpsPlus.Models;
using BankOpsPlus.Repositories;

namespace BankOpsPlus.Services;

/// <summary>
/// Service implementation for incident business logic
/// </summary>
public class IncidentService : IIncidentService
{
    private readonly IIncidentRepository _incidentRepository;

    public IncidentService(IIncidentRepository incidentRepository)
    {
        _incidentRepository = incidentRepository;
    }

    public async Task<IEnumerable<Incident>> GetAllIncidentsAsync()
    {
        return await _incidentRepository.GetIncidentsWithDetailsAsync();
    }

    public async Task<IEnumerable<Incident>> GetIncidentsByFiltersAsync(
        IncidentStatus? status,
        IncidentSeverity? severity)
    {
        var incidents = await _incidentRepository.GetIncidentsWithDetailsAsync();

        if (status.HasValue)
        {
            incidents = incidents.Where(i => i.Status == status.Value);
        }

        if (severity.HasValue)
        {
            incidents = incidents.Where(i => i.Severity == severity.Value);
        }

        return incidents.OrderByDescending(i => i.CreatedAt);
    }

    public async Task<Incident?> GetIncidentByIdAsync(int id)
    {
        var incident = await _incidentRepository.GetByIdAsync(id);

        // Load related entities if needed
        if (incident != null)
        {
            var allIncidents = await _incidentRepository.GetIncidentsWithDetailsAsync();
            incident = allIncidents.FirstOrDefault(i => i.Id == id);
        }

        return incident;
    }

    public async Task<Incident> CreateIncidentAsync(Incident incident, int createdByUserId)
    {
        // Generate reference
        incident.Reference = await _incidentRepository.GenerateIncidentReferenceAsync();

        // Set default values
        incident.Status = IncidentStatus.Open;
        incident.CreatedAt = DateTime.UtcNow;
        incident.CreatedByUserId = createdByUserId;

        return await _incidentRepository.AddAsync(incident);
    }

    public async Task<Incident> UpdateIncidentAsync(Incident incident)
    {
        // If status changed to Resolved, calculate resolution time
        if (incident.Status == IncidentStatus.Resolved && !incident.ResolvedAt.HasValue)
        {
            incident.ResolvedAt = DateTime.UtcNow;
            incident.ResolutionTimeMinutes = (int)(incident.ResolvedAt.Value - incident.CreatedAt).TotalMinutes;
        }

        await _incidentRepository.UpdateAsync(incident);
        return incident;
    }

    public async Task<Incident> ResolveIncidentAsync(int id, string rootCause)
    {
        var incident = await _incidentRepository.GetByIdAsync(id);

        if (incident == null)
        {
            throw new ArgumentException($"Incident with ID {id} not found");
        }

        if (incident.Status == IncidentStatus.Resolved)
        {
            throw new InvalidOperationException("Incident is already resolved");
        }

        incident.Status = IncidentStatus.Resolved;
        incident.RootCause = rootCause;
        incident.ResolvedAt = DateTime.UtcNow;
        incident.ResolutionTimeMinutes = (int)(incident.ResolvedAt.Value - incident.CreatedAt).TotalMinutes;

        await _incidentRepository.UpdateAsync(incident);
        return incident;
    }

    public async Task DeleteIncidentAsync(int id)
    {
        await _incidentRepository.DeleteAsync(id);
    }
}
