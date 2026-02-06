using BankOpsPlus.Models;

namespace BankOpsPlus.Services;

/// <summary>
/// Service for dashboard KPIs and metrics
/// </summary>
public interface IDashboardService
{
    Task<int> GetOpenIncidentsCountAsync();
    Task<int> GetCriticalIncidentsCountAsync();
    Task<double> GetMTTRAsync();
    Task<int> GetJobsInErrorCountAsync();
    Task<IEnumerable<Job>> GetJobsInErrorAsync();
    Task<IEnumerable<Incident>> GetRecentIncidentsAsync(int count = 10);
}
