using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using ticketApp.Models.Dbmodels;

namespace ticketApp.Models.DBmodels;
    public class Person : IdentityUser {
    public override string? UserName { get => base.UserName; set => base.UserName = value; }

    [ForeignKey("EmployeeId")]
    public int EmployeeId { get; set; }
    public Employee Employee { get; set; }
    [ForeignKey("clientId")]
    public int clientId { get; set; }
    public Client client { get; set; }
}
