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
        private UserServices user;
        private ILogger<TicketController> logger;
        private DBContext db;
        private TicketEngine TEngine;
        private UserManager<Person> _userManager;
        public TicketController(UserServices user, TicketEngine TEngine, DBContext db, UserManager<Person> userManager)
        {
            this.user = user;
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
            List<Ticket> tickets = db.Tickets.Where(e => true).Take(10).ToList();
            return View(tickets);
        }
        [HttpPost("submit")]
        [ValidateAntiForgeryToken]
        public IActionResult submit(EnginOutput tickets)
        {
            string email = User.Identity.Name!;
            Employee e = db.Employees.FirstOrDefault(emp => emp.Person.Email == email)!;
            int i = 0;
            tickets.newTickets.ForEach((oldTicket) =>
            {
                Client c = db.Clients.FirstOrDefault(c => c.NickName == oldTicket.Client.NickName)!;
                if (c == null)
                    c = db.Clients.FirstOrDefault(c => c.NickName == "Unknown")!;
                if (tickets.oldTickets.Count > 0)
                    tickets.oldTickets[i].Client = c;
                tickets.newTickets[i++].Client = c;
            });
            if (!tickets.isOldExistinDB)
            {
                foreach (Ticket t in tickets.oldTickets)
                {
                    TEngine.SaveTicketToDB(t);//do math for employee and so on
                }
            }
            foreach (Ticket t in tickets.newTickets)
            {
                TEngine.SaveTicketToDB(t);//do math for employee and so on
            }
            if (tickets.oldTickets.Count != 0)
            {
                List<ReIssuedTickets> ts = tickets.newTickets.Select((t, i) =>
                {
                    return new ReIssuedTickets()
                    {
                        OldTnum = tickets.oldTickets[i].TNum,
                        NewTicket = t
                    };
                }).ToList();
                db.ReIssuedTickets.AddRange(ts);
            }
            db.SaveChanges();
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
