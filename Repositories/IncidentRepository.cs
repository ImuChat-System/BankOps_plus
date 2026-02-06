using BankOpsPlus.Data;
using BankOpsPlus.Models;
using Microsoft.EntityFrameworkCore;

namespace BankOpsPlus.Repositories;

/// <summary>
/// Repository implementation for Incident-specific data access
/// </summary>
public class IncidentRepository : Repository<Incident>, IIncidentRepository
{
    public IncidentRepository(BankOpsDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Incident>> GetIncidentsWithDetailsAsync()
    {
        return await _dbSet
            .Include(i => i.Application)
            .Include(i => i.Job)
            .Include(i => i.CreatedBy)
            .Include(i => i.AssignedTo)
            .OrderByDescending(i => i.CreatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<Incident>> GetOpenIncidentsAsync()
    {
        return await _dbSet
            .Include(i => i.Application)
            .Where(i => i.Status == IncidentStatus.Open || i.Status == IncidentStatus.InProgress)
            .OrderByDescending(i => i.CreatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<Incident>> GetCriticalIncidentsAsync()
    {
        return await _dbSet
            .Include(i => i.Application)
            .Where(i => i.Severity == IncidentSeverity.Critical && i.Status != IncidentStatus.Resolved)
            .OrderByDescending(i => i.CreatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<Incident>> GetIncidentsByStatusAsync(IncidentStatus status)
    {
        return await _dbSet
            .Include(i => i.Application)
            .Include(i => i.Job)
            .Where(i => i.Status == status)
            .OrderByDescending(i => i.CreatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<Incident>> GetIncidentsBySeverityAsync(IncidentSeverity severity)
    {
        return await _dbSet
            .Include(i => i.Application)
            .Include(i => i.Job)
            .Where(i => i.Severity == severity)
            .OrderByDescending(i => i.CreatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<Incident>> GetRecentIncidentsAsync(int count = 10)
    {
        return await _dbSet
            .Include(i => i.Application)
            .Include(i => i.Job)
            .OrderByDescending(i => i.CreatedAt)
            .Take(count)
            .ToListAsync();
    }

    public async Task<double> CalculateMTTRAsync()
    {
        var resolvedIncidents = await _dbSet
            .Where(i => i.Status == IncidentStatus.Resolved && i.ResolutionTimeMinutes.HasValue)
            .ToListAsync();

        return resolvedIncidents.Any()
            ? resolvedIncidents.Average(i => i.ResolutionTimeMinutes ?? 0)
            : 0;
    }

    public async Task<string> GenerateIncidentReferenceAsync()
    {
        var count = await _dbSet.CountAsync();
        var year = DateTime.UtcNow.Year;
        return $"INC-{year}-{(count + 1):D3}";
    }
}
