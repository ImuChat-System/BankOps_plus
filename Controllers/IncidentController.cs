using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BankOpsPlus.Data;
using BankOpsPlus.Models;

namespace BankOpsPlus.Controllers;

public class IncidentController : Controller
{
    private readonly BankOpsDbContext _context;

    public IncidentController(BankOpsDbContext context)
    {
        _context = context;
    }

    // GET: Incident
    public async Task<IActionResult> Index(string? status, string? severity)
    {
        var incidents = _context.Incidents
            .Include(i => i.Application)
            .Include(i => i.Job)
            .Include(i => i.CreatedBy)
            .Include(i => i.AssignedTo)
            .AsQueryable();

        // Apply filters
        if (!string.IsNullOrEmpty(status) && Enum.TryParse<IncidentStatus>(status, out var statusEnum))
        {
            incidents = incidents.Where(i => i.Status == statusEnum);
        }

        if (!string.IsNullOrEmpty(severity) && Enum.TryParse<IncidentSeverity>(severity, out var severityEnum))
        {
            incidents = incidents.Where(i => i.Severity == severityEnum);
        }

        var result = await incidents.OrderByDescending(i => i.CreatedAt).ToListAsync();
        
        ViewBag.CurrentStatus = status;
        ViewBag.CurrentSeverity = severity;
        
        return View(result);
    }

    // GET: Incident/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var incident = await _context.Incidents
            .Include(i => i.Application)
            .Include(i => i.Job)
            .Include(i => i.CreatedBy)
            .Include(i => i.AssignedTo)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (incident == null)
        {
            return NotFound();
        }

        return View(incident);
    }

    // GET: Incident/Create
    public IActionResult Create()
    {
        ViewBag.Applications = _context.Applications.ToList();
        ViewBag.Jobs = _context.Jobs.ToList();
        ViewBag.Users = _context.Users.Where(u => u.IsActive).ToList();
        return View();
    }

    // POST: Incident/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("ApplicationId,JobId,Severity,Description,AssignedToUserId")] Incident incident)
    {
        // Generate reference
        var incidentCount = await _context.Incidents.CountAsync();
        incident.Reference = $"INC-{DateTime.UtcNow.Year}-{(incidentCount + 1):D3}";
        
        // Set default values
        incident.Status = IncidentStatus.Open;
        incident.CreatedAt = DateTime.UtcNow;
        incident.CreatedByUserId = 1; // Default to admin for now (should be from session in production)

        _context.Add(incident);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    // GET: Incident/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var incident = await _context.Incidents.FindAsync(id);
        if (incident == null)
        {
            return NotFound();
        }

        ViewBag.Applications = _context.Applications.ToList();
        ViewBag.Jobs = _context.Jobs.ToList();
        ViewBag.Users = _context.Users.Where(u => u.IsActive).ToList();
        
        return View(incident);
    }

    // POST: Incident/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Reference,ApplicationId,JobId,Severity,Status,Description,RootCause,AssignedToUserId,CreatedAt,CreatedByUserId")] Incident incident)
    {
        if (id != incident.Id)
        {
            return NotFound();
        }

        // If status changed to Resolved, calculate resolution time
        if (incident.Status == IncidentStatus.Resolved && !incident.ResolvedAt.HasValue)
        {
            incident.ResolvedAt = DateTime.UtcNow;
            incident.ResolutionTimeMinutes = (int)(incident.ResolvedAt.Value - incident.CreatedAt).TotalMinutes;
        }

        try
        {
            _context.Update(incident);
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!IncidentExists(incident.Id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }
        
        return RedirectToAction(nameof(Index));
    }

    private bool IncidentExists(int id)
    {
        return _context.Incidents.Any(e => e.Id == id);
    }
}
