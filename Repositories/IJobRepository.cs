using BankOpsPlus.Models;

namespace BankOpsPlus.Repositories;

/// <summary>
/// Repository interface for Job-specific operations
/// </summary>
public interface IJobRepository : IRepository<Job>
{
    Task<IEnumerable<Job>> GetJobsWithApplicationAsync();
    Task<IEnumerable<Job>> GetJobsInErrorAsync();
    Task<IEnumerable<Job>> GetJobsByApplicationIdAsync(int applicationId);
}
