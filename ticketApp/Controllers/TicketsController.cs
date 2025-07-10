using Microsoft.AspNetCore.Mvc;
using ticketApp.Models.Dbmodels;
using ticketApp.Models.DBmodels;
using ticketApp.Services;

namespace ticketApp.Controllers
{
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
        public IActionResult Tickets(Ticket? ticket)
        {
            return View(new List<Ticket>());
        }
        //Create
        [HttpGet("create")]
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost("create")]
        public ActionResult Create_Post([FromForm] string input)
        {
            
            Client client = new()
            {
                Name = "New User",
                Id = 1,
                PhoneNumber = "phone",
            };
            IssueCompany issueCompany = new()
            {
                Name = "IATA",
                Balance = 0,
                Id = 1,
            };
            
            TEngine.Intializer(input);
            
            return View("~/Views/Components/PieChart.cshtml");
        }
        public ActionResult Index()
        {
            return View();
        }
        //Update
        //Delete


    }
}
