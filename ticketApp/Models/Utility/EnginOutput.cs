using ticketApp.Models.DBmodels;
namespace ticketApp.Models.Utility {
    public class EnginOutput {
        public bool isOldExistinDB { get; set; } = true;
        public List<Ticket> oldTickets { get; set; } = new();
        public List<Ticket> newTickets { get; set; } = new();
    }
}
