using BankOpsPlus.Data;
using BankOpsPlus.Models;
using Microsoft.EntityFrameworkCore;

namespace BankOpsPlus.Repositories;

/// <summary>
/// Repository implementation for Job-specific data access
/// </summary>
public class JobRepository : Repository<Job>, IJobRepository
{
    public JobRepository(BankOpsDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Job>> GetJobsWithApplicationAsync()
    {
        return await _dbSet
            .Include(j => j.Application)
            .OrderBy(j => j.Application.Name)
            .ThenBy(j => j.Name)
            .ToListAsync();
    }

    public async Task<IEnumerable<Job>> GetJobsInErrorAsync()
    {
        return await _dbSet
            .Include(j => j.Application)
            .Where(j => j.Status == JobStatus.Error)
            .OrderByDescending(j => j.LastExecution)
            .ToListAsync();
    }

    public async Task<IEnumerable<Job>> GetJobsByApplicationIdAsync(int applicationId)
    {
        return await _dbSet
            .Include(j => j.Application)
            .Where(j => j.ApplicationId == applicationId)
            .OrderBy(j => j.Name)
            .ToListAsync();
    }
}
