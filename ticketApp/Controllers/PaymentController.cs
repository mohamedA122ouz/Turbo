using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using ticketApp.Models.DBmodels;

namespace ticketApp.Controllers
{
    public class PaymentController : Controller
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            ViewData["title"] = "payment";
            base.OnActionExecuting(context);
        }
        // GET: PaymentController
        private DBContext db;
        public PaymentController(DBContext db)
        {
            this.db = db;
        }
        public ActionResult Index()
        {
            
            //Employees
            List<Employee> employees = db.Employees.Include(emp=>emp.Person).Take(10).ToList();
            //broker
            List<Broker> brokers = db.Brokers.Take(10).ToList();
            //issue ticket
            List<IssueCompany> issueCompanies = db.IssueCompanies.Take(10).ToList();
            ViewData["employees"] = employees;
            ViewData["brokers"] = brokers;
            ViewData["issueCompanies"] = issueCompanies;
            return View();
        }

    }
}
