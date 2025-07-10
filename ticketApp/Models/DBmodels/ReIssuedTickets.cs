using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ticketApp.Models.DBmodels;

public class ReIssuedTickets
{
    public int Id { get; set; }
    public string OldTnum { get; set; }
    public int NewTicketId { get; set; }
    public Ticket NewTicket { get; set; }
}
