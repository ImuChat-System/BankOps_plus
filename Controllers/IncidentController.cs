using BankOpsPlus.Data;
using BankOpsPlus.Models;
using BankOpsPlus.Services;
using Microsoft.AspNetCore.Mvc;

namespace BankOpsPlus.Controllers;

public class IncidentController : Controller
{
    private readonly IIncidentService _incidentService;
    private readonly BankOpsDbContext _context; // Still needed for dropdowns

    public IncidentController(IIncidentService incidentService, BankOpsDbContext context)
    {
        _incidentService = incidentService;
        _context = context;
    }

    // GET: Incident
    public async Task<IActionResult> Index(string? status, string? severity)
    {
        IncidentStatus? statusEnum = null;
        IncidentSeverity? severityEnum = null;

        if (!string.IsNullOrEmpty(status) && Enum.TryParse<IncidentStatus>(status, out var parsedStatus))
        {
            statusEnum = parsedStatus;
        }

        if (!string.IsNullOrEmpty(severity) && Enum.TryParse<IncidentSeverity>(severity, out var parsedSeverity))
        {
            severityEnum = parsedSeverity;
        }

        var incidents = await _incidentService.GetIncidentsByFiltersAsync(statusEnum, severityEnum);

        ViewBag.CurrentStatus = status;
        ViewBag.CurrentSeverity = severity;

        return View(incidents);
    }

    // GET: Incident/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var incident = await _incidentService.GetIncidentByIdAsync(id.Value);

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
        // TODO: Get current user ID from session instead of hardcoded value
        const int currentUserId = 1; // Admin user for now

        await _incidentService.CreateIncidentAsync(incident, currentUserId);
        return RedirectToAction(nameof(Index));
    }

    // GET: Incident/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var incident = await _incidentService.GetIncidentByIdAsync(id.Value);
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

        try
        {
            await _incidentService.UpdateIncidentAsync(incident);
        }
        catch (ArgumentException)
        {
            return NotFound();
        }

        return RedirectToAction(nameof(Index));
    }
}
