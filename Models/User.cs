using System.ComponentModel.DataAnnotations;

namespace BankOpsPlus.Models;

public enum UserRole
{
    Admin,
    Support,
    ReadOnly
}

public class User
{
    public int Id { get; set; }
    
    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;
    
    [Required]
    [EmailAddress]
    [StringLength(200)]
    public string Email { get; set; } = string.Empty;
    
    [Required]
    public string PasswordHash { get; set; } = string.Empty;
    
    [Required]
    public UserRole Role { get; set; } = UserRole.ReadOnly;
    
    public bool IsActive { get; set; } = true;
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public DateTime? LastLoginAt { get; set; }
    
    // Navigation properties
    public ICollection<Incident> CreatedIncidents { get; set; } = new List<Incident>();
    public ICollection<Incident> AssignedIncidents { get; set; } = new List<Incident>();
}
