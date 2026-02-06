using BankOpsPlus.Models;
using Microsoft.EntityFrameworkCore;

namespace BankOpsPlus.Data;

public class BankOpsDbContext : DbContext
{
    public BankOpsDbContext(DbContextOptions<BankOpsDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Application> Applications { get; set; } = null!;
    public DbSet<Job> Jobs { get; set; } = null!;
    public DbSet<Incident> Incidents { get; set; } = null!;
    public DbSet<ChangeRequest> ChangeRequests { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure User entity
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.Email).IsUnique();
            entity.Property(e => e.Role).HasConversion<string>();
        });

        // Configure Application entity
        modelBuilder.Entity<Application>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.Code).IsUnique();
            entity.Property(e => e.Environment).HasConversion<string>();
        });

        // Configure Job entity
        modelBuilder.Entity<Job>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Frequency).HasConversion<string>();
            entity.Property(e => e.Status).HasConversion<string>();

            entity.HasOne(e => e.Application)
                .WithMany(a => a.Jobs)
                .HasForeignKey(e => e.ApplicationId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Configure Incident entity
        modelBuilder.Entity<Incident>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.Reference).IsUnique();
            entity.Property(e => e.Severity).HasConversion<string>();
            entity.Property(e => e.Status).HasConversion<string>();

            entity.HasOne(e => e.Application)
                .WithMany(a => a.Incidents)
                .HasForeignKey(e => e.ApplicationId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Job)
                .WithMany(j => j.Incidents)
                .HasForeignKey(e => e.JobId)
                .OnDelete(DeleteBehavior.SetNull);

            entity.HasOne(e => e.CreatedBy)
                .WithMany(u => u.CreatedIncidents)
                .HasForeignKey(e => e.CreatedByUserId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.AssignedTo)
                .WithMany(u => u.AssignedIncidents)
                .HasForeignKey(e => e.AssignedToUserId)
                .OnDelete(DeleteBehavior.SetNull);
        });

        // Configure ChangeRequest entity
        modelBuilder.Entity<ChangeRequest>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Type).HasConversion<string>();
            entity.Property(e => e.Priority).HasConversion<string>();
            entity.Property(e => e.Status).HasConversion<string>();

            entity.HasOne(e => e.Application)
                .WithMany(a => a.ChangeRequests)
                .HasForeignKey(e => e.ApplicationId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Seed data
        SeedData(modelBuilder);
    }

    private void SeedData(ModelBuilder modelBuilder)
    {
        // Seed Users
        modelBuilder.Entity<User>().HasData(
            new User
            {
                Id = 1,
                Name = "Admin User",
                Email = "admin@bankops.local",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin123!"),
                Role = UserRole.Admin,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },
            new User
            {
                Id = 2,
                Name = "Support Team",
                Email = "support@bankops.local",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Support123!"),
                Role = UserRole.Support,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },
            new User
            {
                Id = 3,
                Name = "Reader User",
                Email = "reader@bankops.local",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Reader123!"),
                Role = UserRole.ReadOnly,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            }
        );

        // Seed Applications
        modelBuilder.Entity<Application>().HasData(
            new Application
            {
                Id = 1,
                Code = "PAY-PROD-001",
                Name = "Payment Processing System",
                Environment = ApplicationEnvironment.PROD,
                ResponsiblePerson = "John Doe",
                GlobalStatus = "Active",
                CreatedAt = DateTime.UtcNow
            },
            new Application
            {
                Id = 2,
                Code = "TRADE-PROD-002",
                Name = "Trading Platform",
                Environment = ApplicationEnvironment.PROD,
                ResponsiblePerson = "Jane Smith",
                GlobalStatus = "Active",
                CreatedAt = DateTime.UtcNow
            },
            new Application
            {
                Id = 3,
                Code = "RISK-REC-001",
                Name = "Risk Management",
                Environment = ApplicationEnvironment.REC,
                ResponsiblePerson = "Bob Johnson",
                GlobalStatus = "Testing",
                CreatedAt = DateTime.UtcNow
            },
            new Application
            {
                Id = 4,
                Code = "CRM-DEV-001",
                Name = "Customer Relationship Mgmt",
                Environment = ApplicationEnvironment.DEV,
                ResponsiblePerson = "Alice Brown",
                GlobalStatus = "Development",
                CreatedAt = DateTime.UtcNow
            },
            new Application
            {
                Id = 5,
                Code = "BATCH-PROD-003",
                Name = "Batch Processing Engine",
                Environment = ApplicationEnvironment.PROD,
                ResponsiblePerson = "Charlie Wilson",
                GlobalStatus = "Active",
                CreatedAt = DateTime.UtcNow
            }
        );

        // Seed Jobs
        var baseDate = DateTime.UtcNow.Date.AddDays(-1);
        modelBuilder.Entity<Job>().HasData(
            new Job
            {
                Id = 1,
                Name = "Daily Payment Reconciliation",
                ApplicationId = 1,
                Frequency = JobFrequency.Daily,
                LastExecution = baseDate.AddHours(2),
                Status = JobStatus.OK,
                CreatedAt = DateTime.UtcNow.AddMonths(-3)
            },
            new Job
            {
                Id = 2,
                Name = "Weekly Trade Settlement",
                ApplicationId = 2,
                Frequency = JobFrequency.Weekly,
                LastExecution = baseDate.AddDays(-2),
                Status = JobStatus.OK,
                CreatedAt = DateTime.UtcNow.AddMonths(-2)
            },
            new Job
            {
                Id = 3,
                Name = "Risk Calculation Batch",
                ApplicationId = 3,
                Frequency = JobFrequency.Daily,
                LastExecution = baseDate.AddHours(3),
                Status = JobStatus.Error,
                CreatedAt = DateTime.UtcNow.AddMonths(-1)
            },
            new Job
            {
                Id = 4,
                Name = "Monthly Account Statements",
                ApplicationId = 5,
                Frequency = JobFrequency.Monthly,
                LastExecution = baseDate.AddDays(-7),
                Status = JobStatus.OK,
                CreatedAt = DateTime.UtcNow.AddMonths(-6)
            },
            new Job
            {
                Id = 5,
                Name = "Hourly Data Sync",
                ApplicationId = 1,
                Frequency = JobFrequency.Daily,
                LastExecution = baseDate.AddHours(1),
                Status = JobStatus.InProgress,
                CreatedAt = DateTime.UtcNow.AddMonths(-1)
            }
        );

        // Seed Incidents
        modelBuilder.Entity<Incident>().HasData(
            new Incident
            {
                Id = 1,
                Reference = "INC-2026-001",
                ApplicationId = 1,
                JobId = 1,
                Severity = IncidentSeverity.Medium,
                Status = IncidentStatus.Resolved,
                Description = "Payment reconciliation delayed by 2 hours",
                RootCause = "Network latency with external provider",
                CreatedAt = DateTime.UtcNow.AddDays(-5),
                ResolvedAt = DateTime.UtcNow.AddDays(-5).AddHours(2),
                ResolutionTimeMinutes = 120,
                CreatedByUserId = 2,
                AssignedToUserId = 1
            },
            new Incident
            {
                Id = 2,
                Reference = "INC-2026-002",
                ApplicationId = 3,
                JobId = 3,
                Severity = IncidentSeverity.Critical,
                Status = IncidentStatus.Open,
                Description = "Risk calculation batch failing - data corruption suspected",
                CreatedAt = DateTime.UtcNow.AddHours(-4),
                CreatedByUserId = 2,
                AssignedToUserId = 1
            },
            new Incident
            {
                Id = 3,
                Reference = "INC-2026-003",
                ApplicationId = 2,
                Severity = IncidentSeverity.Low,
                Status = IncidentStatus.InProgress,
                Description = "Minor UI glitch in trading dashboard",
                CreatedAt = DateTime.UtcNow.AddDays(-2),
                CreatedByUserId = 3,
                AssignedToUserId = 2
            }
        );

        // Seed ChangeRequests
        modelBuilder.Entity<ChangeRequest>().HasData(
            new ChangeRequest
            {
                Id = 1,
                ApplicationId = 1,
                Type = ChangeRequestType.Enhancement,
                Priority = ChangeRequestPriority.High,
                Status = ChangeRequestStatus.Approved,
                Description = "Add support for new payment provider",
                EstimatedImpact = "2 weeks development + 1 week testing",
                CreatedAt = DateTime.UtcNow.AddMonths(-1)
            },
            new ChangeRequest
            {
                Id = 2,
                ApplicationId = 3,
                Type = ChangeRequestType.Bug,
                Priority = ChangeRequestPriority.High,
                Status = ChangeRequestStatus.InProgress,
                Description = "Fix data corruption issue in risk calculations",
                EstimatedImpact = "Critical - 3 days",
                CreatedAt = DateTime.UtcNow.AddDays(-3)
            },
            new ChangeRequest
            {
                Id = 3,
                ApplicationId = 4,
                Type = ChangeRequestType.Enhancement,
                Priority = ChangeRequestPriority.Medium,
                Status = ChangeRequestStatus.Pending,
                Description = "Implement customer segmentation feature",
                EstimatedImpact = "1 month development",
                CreatedAt = DateTime.UtcNow.AddDays(-7)
            }
        );
    }
}
