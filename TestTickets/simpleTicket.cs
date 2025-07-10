using System;

namespace TestTickets.test;

public class simpleTicket
{
    public required string PNR { get; set; }
    public required string Airline { get; set; }
    public required string TNum { get; set; }
    public required decimal Price { get; set; }
    public required string Client { get; set; }
    public required string IssueCompany { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}
