using System.ComponentModel.DataAnnotations;

namespace BankOpsPlus.Models;

public enum JobFrequency
{
    Daily,
    Weekly,
    Monthly
}

public enum JobStatus
{
    OK,
    InProgress,
    Error
}

public class Job
{
    public int Id { get; set; }
    
    [Required]
    [StringLength(200)]
    public string Name { get; set; } = string.Empty;
    
    [Required]
    public int ApplicationId { get; set; }
    
    [Required]
    public JobFrequency Frequency { get; set; } = JobFrequency.Daily;
    
    public DateTime? LastExecution { get; set; }
    
    [Required]
    public JobStatus Status { get; set; } = JobStatus.OK;
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    // Navigation properties
    public Application Application { get; set; } = null!;
    public ICollection<Incident> Incidents { get; set; } = new List<Incident>();
}
