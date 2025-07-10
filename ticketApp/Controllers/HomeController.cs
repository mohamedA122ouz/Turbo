using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ticketApp.Models;
using ticketApp.Models.Dbmodels;
using ticketApp.Models.DBmodels;

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
        Employee me = db.Employees.FirstOrDefault(e => e.Name == User.Identity.Name)!;
        List<Employee> em = db.Employees.Take(10).Where(em => em!=me).ToList();
        return View(em);
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
