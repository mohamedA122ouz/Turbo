using System;
using System.ComponentModel.DataAnnotations.Schema;
using ticketApp.Models.DBmodels;
using ticketApp.Models.Utility;

namespace ticketApp.Models;

public class Payment
{
    public int Id { get; set; }
    public required string TransactionId { get; set; }
    public required decimal Amount { get; set; }
    public required DateTime TransactionDate { get; set; }
    public required string PaymentMethod { get; set; }
    public required PaymentStatus Status { get; set; }
    [ForeignKey("TicketId")]
    public int TicketId { get; set; }
    public required Ticket Ticket { get; set; }

}
