using BankOpsPlus.Models;
using BankOpsPlus.Repositories;

namespace BankOpsPlus.Services;

/// <summary>
/// Service implementation for dashboard metrics and KPIs
/// </summary>
public class DashboardService : IDashboardService
{
    private readonly IIncidentRepository _incidentRepository;
    private readonly IJobRepository _jobRepository;

    public DashboardService(
        IIncidentRepository incidentRepository,
        IJobRepository jobRepository)
    {
        _incidentRepository = incidentRepository;
        _jobRepository = jobRepository;
    }

    public async Task<int> GetOpenIncidentsCountAsync()
    {
        var openIncidents = await _incidentRepository.GetOpenIncidentsAsync();
        return openIncidents.Count();
    }

    public async Task<int> GetCriticalIncidentsCountAsync()
    {
        var criticalIncidents = await _incidentRepository.GetCriticalIncidentsAsync();
        return criticalIncidents.Count();
    }

    public async Task<double> GetMTTRAsync()
    {
        return await _incidentRepository.CalculateMTTRAsync();
    }

    public async Task<int> GetJobsInErrorCountAsync()
    {
        var jobsInError = await _jobRepository.GetJobsInErrorAsync();
        return jobsInError.Count();
    }

    public async Task<IEnumerable<Job>> GetJobsInErrorAsync()
    {
        return await _jobRepository.GetJobsInErrorAsync();
    }

    public async Task<IEnumerable<Incident>> GetRecentIncidentsAsync(int count = 10)
    {
        return await _incidentRepository.GetRecentIncidentsAsync(count);
    }
}
