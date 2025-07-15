using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
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
        public TicketController(UserServices user, TicketEngine TEngine, DBContext db)
        {
            this.user = user;
            this.db = db;
            this.TEngine = TEngine;
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
        public IActionResult submit(EnginOutput tickets)
        {
            if (!tickets.isOldExistinDB)
            {
                db.Tickets.AddRange(tickets.oldTickets);
            }
            db.Tickets.AddRange(tickets.newTickets);
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
            return View("~/Views/Ticket/Tickets.cshtml", tickets);
        }
        //Create
        [HttpGet("create")]
        public ActionResult Create(EnginOutput outp)
        {
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
