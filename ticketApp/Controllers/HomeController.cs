using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ticketApp.Models;

using ticketApp.Models.DBmodels;
using ticketApp.Models.Utility;

namespace ticketApp.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly DBContext db;

    public HomeController(ILogger<HomeController> logger, DBContext db)
    {
        _logger = logger;
        this.db = db;
    }

    public IActionResult Index()
    {
        return View();
    }
    [HttpGet("employees")]
    public IActionResult Employees()
    {
        // This is a placeholder for the employees view.
        ViewData["title"] = "employees";
        Employee me = db.Employees.FirstOrDefault(e => e.Name == User.Identity.Name)!;
        List<Employee> em = db.Employees.Include(e => e.Person).Take(10).Where(em => em != me).ToList();
        return View(em);
    }
    [HttpGet("employee")]
    public IActionResult Employee(int id)
    {
        ViewData["title"] = "employees";
        Employee me = db.Employees.Include(em => em.Tickets).ThenInclude(em=>em.IssueCompany).FirstOrDefault(e => e.Id == id)!;
        List<pieAnalysis> pieChartData = me.Tickets.GroupBy(t => new { t.IssueCompanyId, t.IssueCompany.Name }).Select(
            g => new pieAnalysis()
            {
                name = g.Key.Name,
                percentage = g.Count()
            }).ToList();
        double total = pieChartData.Sum(g => g.percentage);
        pieChartData = pieChartData.Select(
            g => new pieAnalysis()
            {
                name = g.name,
                percentage = g.percentage / total * 100
            }
        ).ToList();
        ViewData["pie"] = pieChartData;
        return View(me);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
