using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace ticketApp.Models.DBmodels;

public class Client
{
    public int Id { get; set; }
    [Required]
    public string Name { get; set; }
    public string NickName { get; set; } = string.Empty;
    public string PhoneNumber { get; set; }
    [ForeignKey("personId")]
    public string personId { get; set; }
    public Person Person { get; set; }
    public List<Ticket> Ticket { get; set; } = new List<Ticket>();
}
