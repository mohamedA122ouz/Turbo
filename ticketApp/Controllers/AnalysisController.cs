using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;

using ticketApp.Models.DBmodels;
using ticketApp.Models.Utility;

namespace ticketApp.Controllers
{
    [Route("/analysis")]
    public class Analysis : Controller
    {
        private readonly DBContext db;

        public Analysis(DBContext db)
        {
            this.db = db;
        }
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            ViewData["title"] = "Analysis";
            base.OnActionExecuting(context);
        }
        // GET: Analysis
        public ActionResult Index()
        {
            List<Employee> employees = db.Employees.Include(e => e.Person).Take(10).OrderByDescending(emp => emp.Tickets.Where(t => t.CreatedAt > DateTime.Now.AddDays(-30)).ToList().Count).ToList();
            List<pieAnalysis> pieChartData = db.Tickets.GroupBy(t => new { t.IssueCompanyId, t.IssueCompany.Name }).Select(
                g => new pieAnalysis()
                {
                    name = g.Key.Name,
                    percentage = g.Count()
                }
                ).ToList();
            double total = pieChartData.Sum(g => g.percentage);
            pieChartData = pieChartData.Select(
                g => new pieAnalysis()
                {
                    name = g.name,
                    percentage = g.percentage / total * 100
                }
            ).ToList();
            ViewData["employees"] = employees;
            ViewData["pieChartData"] = pieChartData;
            return View();
        }

    }
}
