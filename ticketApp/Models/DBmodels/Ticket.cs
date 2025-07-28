

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;


namespace ticketApp.Models.DBmodels;

public class Ticket
{
    public int Id { get; set; }
    public required string PNR { get; set; }
    public required string Airline { get; set; }
    public required string Destination { get; set; }
    public required bool isAReIssued { get; set; } = false;
    public required string TNum { get; set; }
    [ForeignKey("EmployeeId")]
    public int EmployeeId { get; set; }
    [Required]
    public Employee Employee { get; set; }
    [ForeignKey("BrokerId")]
    public int? BrokerId { get; set; }
    public Broker? Broker { get; set; }
    public required decimal NetPrice { get; set; }
    public required decimal SellPrice { get; set; }
    public List<Payment> Payments { get; set; } = new List<Payment>();
    [ForeignKey("ClientId")]
    public int ClientId { get; set; } = 1;
    [Required]
    public Client Client { get; set; }
    [ForeignKey("IssueCompanyId")]
    public int IssueCompanyId { get; set; }
    [Required]
    public IssueCompany IssueCompany { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;

}
