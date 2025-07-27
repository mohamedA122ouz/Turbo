using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using ticketApp.Models.DBmodels;

namespace ticketApp.Controllers
{
    [Route("payment")]
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
            List<Employee> employees = db.Employees.Include(emp => emp.Person).Take(10).ToList();
            //broker
            List<Broker> brokers = db.Brokers.Take(10).ToList();
            //issue ticket
            List<IssueCompany> issueCompanies = db.IssueCompanies.Take(10).ToList();
            ViewData["employees"] = employees;
            ViewData["brokers"] = brokers;
            ViewData["issueCompanies"] = issueCompanies;
            return View();
        }
        [HttpGet("choose")]
        public ActionResult Choose(string type, int id)
        {
            ViewData["type"] = type;
            ViewData["id"] = id;
            return View();
        }
        [HttpGet("cash")]
        public ActionResult cash(string type, int id)
        {

            switch (type)
            {
                case "employee":
                    ViewData["employee"] = db.Employees.Include(e => e.Tickets).FirstOrDefault(e => e.Id == id);
                    break;
                case "broker":
                    ViewData["broker"] = db.Brokers.Include(e => e.Tickets).FirstOrDefault(e => e.Id == id);
                    break;
                case "issue":
                    ViewData["issue"] = db.IssueCompanies.FirstOrDefault(e => e.Id == id);
                    break;
                default:
                    break;
            }
            return View("bill");
        }
        [HttpPost]
        public IActionResult ConfirmPayment(List<int> tickets, decimal confirmedAmount)
        {
            return Redirect("home");
        }

    }
}
