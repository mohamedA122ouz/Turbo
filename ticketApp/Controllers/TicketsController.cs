using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using ticketApp.Models.DBmodels;
using ticketApp.Models.Utility;
using ticketApp.Services;

namespace ticketApp.Controllers
{
    [Authorize]
    [Route("tickets")]
    public class TicketController : Controller
    {
        private AccountantServices accountant;
        private ILogger<TicketController> logger;
        private DBContext db;
        private TicketEngine TEngine;
        private UserManager<Person> _userManager;
        public TicketController(AccountantServices accountant, TicketEngine TEngine, DBContext db, UserManager<Person> userManager)
        {
            this.accountant = accountant;
            this.db = db;
            this.TEngine = TEngine;
            _userManager = userManager;
        }
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            ViewData["title"] = "tickets";
            base.OnActionExecuting(context);
        }
        //Read
        [HttpGet("list")]
        public IActionResult list()
        {
            List<Ticket> tickets = db.Tickets.Where(e => e.Payments.Count == 0).Take(10).ToList();
            return View(tickets);
        }
        [HttpPost("submit")]
        [ValidateAntiForgeryToken]
        public IActionResult submit(EnginOutput tickets)
        {
            string email = User.Identity.Name!;
            Employee e = db.Employees.FirstOrDefault(emp => emp.Person.Email == email)!;
            int i = 0;
            db.Entry(tickets.newTickets[0]).Reference(t => t.Client).Load();
            tickets.newTickets.ForEach(el => el.Client = tickets.newTickets[0].Client);
            IssueCompany issueCompany = tickets.newTickets[0].IssueCompany;
            accountant.SellTicket(e, tickets.newTickets[0].Client, issueCompany, tickets);
            List<IssueCompany> issueCompanies = db.IssueCompanies.ToList();
            ViewData["issueCompanies"] = issueCompanies;
            return View("~/Views/Ticket/Tickets.cshtml", tickets);
        }
        //Create
        [HttpGet("create")]
        public ActionResult Create(EnginOutput outp)
        {
            List<IssueCompany> issueCompanies = db.IssueCompanies.ToList();
            ViewData["issueCompanies"] = issueCompanies;
            return View(outp);
        }
        [HttpPost("create")]
        public ActionResult Create_Post([FromForm] string input)
        {
            string email = User.Identity.Name!;
            Employee e = db.Employees.FirstOrDefault(emp => emp.Person.Email == email)!;
            Client client = db.Clients.FirstOrDefault(cl => cl.NickName == "Unknown");
            IssueCompany issueCompany = db.IssueCompanies.FirstOrDefault(i => i.Name == "IATA");
            TEngine.Intializer(input);
            EnginOutput ii = TEngine.createTickets(e, issueCompany, null, client)!;
            if (db.Tickets.FirstOrDefault(t => t.TNum == ii.newTickets[0].TNum) != null)
            {
                ViewData["ToastMessage"] = "This Ticket Already Exist!!";
            }
            List<IssueCompany> issueCompanies = db.IssueCompanies.ToList();
            ViewData["issueCompanies"] = issueCompanies;
            return View("~/Views/Ticket/Tickets.cshtml", ii);
        }
        public ActionResult Index()
        {
            return View();
        }
        //Update
        //Delete


    }
}
