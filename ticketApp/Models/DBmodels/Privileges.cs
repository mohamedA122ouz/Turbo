using System.ComponentModel.DataAnnotations.Schema;
using ticketApp.Models.DBmodels;

namespace ticketApp.Models.Dbmodels;

public class Privileges
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public bool CanCreateTicket { get; set; }
    public bool CanViewTicket { get; set; }
    public bool CanEditTicket { get; set; }
    public bool CanDeleteTicket { get; set; }
    public bool CanCreateBroker { get; set; }
    public bool CanViewBroker { get; set; }
    public bool CanEditBroker { get; set; }
    public bool CanDeleteBroker { get; set; }
    public bool CanShowAnalytics { get; set; }
    public bool CanViewPayments { get; set; }
    public bool CanEditPayments { get; set; }
    public bool CanDeletePayments { get; set; }
    public bool CanCreatePayment { get; set; }
    public bool CanViewClients { get; set; }
    public bool CanEditClients { get; set; }
    public bool CanDeleteClients { get; set; }
    public bool CanViewEmployees { get; set; }
    public bool CanEditEmployees { get; set; }
    public bool CanDeleteEmployees { get; set; }
    public bool CanViewPrivileges { get; set; }
    public bool CanEditPrivileges { get; set; }
    public bool CanDeletePrivileges { get; set; }
    public bool CanViewSettings { get; set; }
    public bool CanEditSettings { get; set; }
    public bool CanDeleteSettings { get; set; }
    public required List<Employee> Employees { get; set; }
}
