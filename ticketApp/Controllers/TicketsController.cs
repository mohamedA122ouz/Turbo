using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ticketApp.Models.Dbmodels;
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
        //Read
        [HttpGet("show")]
        public IActionResult Tickets()
        {
            return View();
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
            EnginOutput ii = TEngine.createTickets(e,issueCompany,null,client)!;
            
            return View("~/Views/Ticket/Tickets.cshtml",ii);
        }
        public ActionResult Index()
        {
            return View();
        }
        //Update
        //Delete


    }
}
