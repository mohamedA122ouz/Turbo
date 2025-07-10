using System;
using ticketApp.Models.DBmodels;

namespace ticketApp.Models.Dbmodels;

public class IssueCompany
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public decimal Balance { get; set; } = 0;
    public List<Ticket> Tickets { get; set; } = new List<Ticket>();
}
