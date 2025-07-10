using System;
using System.Text.RegularExpressions;
using ticketApp.Models.Dbmodels;
using ticketApp.Models.DBmodels;
using ticketApp.Models.Utility;

namespace ticketApp.Services;

public class UserServices
{
    private readonly DBContext db;
    private Ticket? ticket { get; set; }
    private TicketEngine TEngine { get; set; }
    public UserServices(DBContext db,TicketEngine TEngine)
    {
        this.TEngine = TEngine;
        this.db = db;
    }
    public Ticket? GetTicketByPNR(string pnr)
    {
        return db.Tickets.FirstOrDefault(t => t.PNR == pnr);
    }
    
    public void PayFor(Employee emp,decimal value) {
        if (emp == null)
            return;
        emp.Balance += value;
    }
    public void PayFor(Broker b, decimal value) {
        if (b == null)
            return;
        b.Balance += value;
    }
    public void PayFor(IssueCompany company,decimal value) {
        if (company == null)
            return;
        company.Balance += value;
    }


}

