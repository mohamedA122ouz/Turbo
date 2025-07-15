using System;

namespace ticketApp.Models.DBmodels;

public class IssueCompany
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public decimal Balance { get; set; } = 0;
    public List<Ticket> Tickets { get; set; } = new List<Ticket>();
}
