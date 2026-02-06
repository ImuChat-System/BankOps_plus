using BankOpsPlus.Services;
using Microsoft.AspNetCore.Mvc;

namespace BankOpsPlus.Controllers;

public class DashboardController : Controller
{
    private readonly IDashboardService _dashboardService;

    public DashboardController(IDashboardService dashboardService)
    {
        _dashboardService = dashboardService;
    }

    public async Task<IActionResult> Index()
    {
        // Get KPIs from service
        ViewBag.OpenIncidentsCount = await _dashboardService.GetOpenIncidentsCountAsync();
        ViewBag.CriticalIncidentsCount = await _dashboardService.GetCriticalIncidentsCountAsync();
        ViewBag.MTTR = Math.Round(await _dashboardService.GetMTTRAsync(), 1);

        var jobsInError = await _dashboardService.GetJobsInErrorAsync();
        ViewBag.JobsInErrorCount = jobsInError.Count();
        ViewBag.JobsInError = jobsInError;

        ViewBag.RecentIncidents = await _dashboardService.GetRecentIncidentsAsync(10);

        return View();
    }
}
