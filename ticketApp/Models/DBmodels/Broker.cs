using System.ComponentModel.DataAnnotations.Schema;

namespace ticketApp.Models.DBmodels;

public class Broker
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Email { get; set; }
    public required string PhoneNum { get; set; }
    public List<Ticket> Tickets { get; set; } = new List<Ticket>();
    [ForeignKey("EmployeeId")]
    public int EmployeeId { get; set; }
    public Employee Employee { get; set; }
    public decimal Balance { get; set; } = 0;
}
