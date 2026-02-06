using System.ComponentModel.DataAnnotations;

namespace BankOpsPlus.Models;

public enum IncidentSeverity
{
    Low,
    Medium,
    Critical
}

public enum IncidentStatus
{
    Open,
    InProgress,
    Resolved
}

public class Incident
{
    public int Id { get; set; }
    
    [Required]
    [StringLength(50)]
    public string Reference { get; set; } = string.Empty;
    
    [Required]
    public int ApplicationId { get; set; }
    
    public int? JobId { get; set; }
    
    [Required]
    public IncidentSeverity Severity { get; set; } = IncidentSeverity.Medium;
    
    [Required]
    public IncidentStatus Status { get; set; } = IncidentStatus.Open;
    
    [Required]
    public string Description { get; set; } = string.Empty;
    
    public string? RootCause { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public DateTime? ResolvedAt { get; set; }
    
    public int? ResolutionTimeMinutes { get; set; }
    
    [Required]
    public int CreatedByUserId { get; set; }
    
    public int? AssignedToUserId { get; set; }
    
    // Navigation properties
    public Application Application { get; set; } = null!;
    public Job? Job { get; set; }
    public User CreatedBy { get; set; } = null!;
    public User? AssignedTo { get; set; }
}
