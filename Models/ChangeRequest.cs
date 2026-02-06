using System.ComponentModel.DataAnnotations;

namespace BankOpsPlus.Models;

public enum ChangeRequestType
{
    Bug,
    Enhancement
}

public enum ChangeRequestPriority
{
    Low,
    Medium,
    High
}

public enum ChangeRequestStatus
{
    Pending,
    Approved,
    InProgress,
    Done
}

public class ChangeRequest
{
    public int Id { get; set; }
    
    [Required]
    public int ApplicationId { get; set; }
    
    [Required]
    public ChangeRequestType Type { get; set; } = ChangeRequestType.Enhancement;
    
    [Required]
    public ChangeRequestPriority Priority { get; set; } = ChangeRequestPriority.Medium;
    
    [Required]
    public ChangeRequestStatus Status { get; set; } = ChangeRequestStatus.Pending;
    
    [Required]
    public string Description { get; set; } = string.Empty;
    
    public string? EstimatedImpact { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public DateTime? CompletedAt { get; set; }
    
    // Navigation property
    public Application Application { get; set; } = null!;
}
