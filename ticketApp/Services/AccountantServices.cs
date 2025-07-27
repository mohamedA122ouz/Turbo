using System;
using System.Text.RegularExpressions;

using ticketApp.Models.DBmodels;
using ticketApp.Models.Utility;

namespace ticketApp.Services;

public class UserServices
{
    private readonly DBContext db;
    private Ticket? ticket { get; set; }
    private TicketEngine TEngine { get; set; }
    private Balance balance { get; set; }
    public UserServices(DBContext db, TicketEngine TEngine)
    {
        this.TEngine = TEngine;
        this.db = db;
        balance = db.Balance.FirstOrDefault(t => t.Date.Month == DateTime.Now.Month);
    }
    // > on selling for customer
    //      employee.balance -=  sell;
    //      customer.balance -= sell;
    //      issueCompany.balance -= net;
    //      balance.virtualProfit += sell - net
    //      save ticket to db
    public void SellTicket(Employee employee, Client client, IssueCompany issueCompany, EnginOutput enginOutput)
    {
        //buisness logic
        decimal sell = enginOutput.newTickets.Sum(t => t.SellPrice) + enginOutput.oldTickets.Sum(t => t.SellPrice);
        decimal net = enginOutput.newTickets.Sum(t => t.NetPrice) + enginOutput.oldTickets.Sum(t => t.NetPrice);
        employee.Balance -= sell;
        client.balance -= sell;
        issueCompany.Balance -= net;
        balance.VirtualProfit = sell - net;
        //tickets save
        db.Tickets.AddRange(enginOutput.newTickets);
        db.Tickets.AddRange(enginOutput.oldTickets);
        // Update data base
        db.SaveChanges();
    }
}

