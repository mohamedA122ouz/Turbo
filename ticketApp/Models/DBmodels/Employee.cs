using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ticketApp.Models.Utility;

namespace ticketApp.Models.DBmodels;

public class Employee
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string PhoneNum { get; set; }
    public List<Ticket> Tickets { get; set; }
    [Required]
    public EmployeeType EmployeeType { get; set; }
    public List<Broker> Prokers { get; set; } = new List<Broker>();
    [ForeignKey("PrivilegesId")]
    public int PrivilegesId { get; set; } = 2;// Default to User privileges "Ticket Agent"
    public Privileges Privileges { get; set; }
    public decimal Salary { get; set; }
    public string? ImagePath { get; set; }
    public decimal Balance { get; set; } = 0;
    [ForeignKey("personId")]
    public string personId { get; set; }
    public Person Person { get; set; }
}
