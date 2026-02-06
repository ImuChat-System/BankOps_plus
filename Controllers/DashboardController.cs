using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BankOpsPlus.Data;
using BankOpsPlus.Models;

namespace BankOpsPlus.Controllers;

public class DashboardController : Controller
{
    private readonly BankOpsDbContext _context;

    public DashboardController(BankOpsDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        // Calculate KPIs
        var openIncidentsCount = await _context.Incidents
            .Where(i => i.Status == IncidentStatus.Open || i.Status == IncidentStatus.InProgress)
            .CountAsync();

        var criticalIncidentsCount = await _context.Incidents
            .Where(i => i.Severity == IncidentSeverity.Critical && i.Status != IncidentStatus.Resolved)
            .CountAsync();

        var recentIncidents = await _context.Incidents
            .Include(i => i.Application)
            .Include(i => i.Job)
            .OrderByDescending(i => i.CreatedAt)
            .Take(10)
            .ToListAsync();

        var jobsInError = await _context.Jobs
            .Include(j => j.Application)
            .Where(j => j.Status == JobStatus.Error)
            .ToListAsync();

        // Calculate MTTR (Mean Time To Resolution) - only for resolved incidents
        var resolvedIncidentsWithTime = await _context.Incidents
            .Where(i => i.Status == IncidentStatus.Resolved && i.ResolutionTimeMinutes.HasValue)
            .ToListAsync();

        double mttr = resolvedIncidentsWithTime.Any()
            ? resolvedIncidentsWithTime.Average(i => i.ResolutionTimeMinutes ?? 0)
            : 0;

        ViewBag.OpenIncidentsCount = openIncidentsCount;
        ViewBag.CriticalIncidentsCount = criticalIncidentsCount;
        ViewBag.MTTR = Math.Round(mttr, 1);
        ViewBag.JobsInErrorCount = jobsInError.Count;
        ViewBag.JobsInError = jobsInError;
        ViewBag.RecentIncidents = recentIncidents;

        return View();
    }
}
