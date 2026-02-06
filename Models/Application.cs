using System.ComponentModel.DataAnnotations;

namespace BankOpsPlus.Models;

public enum ApplicationEnvironment
{
    DEV,
    REC,
    PROD
}

public class Application
{
    public int Id { get; set; }

    [Required]
    [StringLength(20)]
    public string Code { get; set; } = string.Empty;

    [Required]
    [StringLength(200)]
    public string Name { get; set; } = string.Empty;

    [Required]
    public ApplicationEnvironment Environment { get; set; } = ApplicationEnvironment.DEV;

    [StringLength(100)]
    public string? ResponsiblePerson { get; set; }

    [StringLength(50)]
    public string GlobalStatus { get; set; } = "Active";

    public bool IsActive { get; set; } = true;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public ICollection<Job> Jobs { get; set; } = new List<Job>();
    public ICollection<Incident> Incidents { get; set; } = new List<Incident>();
    public ICollection<ChangeRequest> ChangeRequests { get; set; } = new List<ChangeRequest>();
}
