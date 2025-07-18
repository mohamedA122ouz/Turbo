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
            PersonCard personCard = new PersonCard()
            {
                Name = "John Doe",
                Message = "Employee of the month",
                ImageUrl = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcRRGG8jKBg7R8mbgNJRLjbKG_DMwOorZxmhFQ&s",
                FocusedButton = "View Profile",
                ButtonText = "Contact"
            };
            ViewData["personCard"] = personCard;
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
