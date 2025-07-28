using System;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Blazor;
using ticketApp.Models;
using ticketApp.Models.DBmodels;
using ticketApp.Models.Utility;

namespace ticketApp.Services;

public class AccountantServices
{
    private readonly DBContext db;
    private Ticket? ticket { get; set; }
    private TicketEngine TEngine { get; set; }
    private Balance balance { get; set; }
    static string Cash = "CASH";
    static string Card = "CARD";
    public AccountantServices(DBContext db, TicketEngine TEngine)
    {
        this.TEngine = TEngine;
        this.db = db;
        balance = db.Balance.FirstOrDefault(t => t.Date.Month == DateTime.Now.Month);
        if (balance == null)
        {
            balance = db.Balance.FirstOrDefault(t => t.Date.Month == DateTime.Now.Month - 1);
            if (balance == null)
            {
                balance = new()
                {
                    Date = DateTime.Now,
                    Value = 0,
                    VirtualProfit = 0
                };
                db.Balance.Add(balance);
                db.SaveChanges();
            }

        }
    }
    public bool checkTicketInput(List<Ticket> tickets)
    {
        foreach (Ticket ticket in tickets)
        {
            if (string.IsNullOrWhiteSpace(ticket.PNR) || string.IsNullOrWhiteSpace(ticket.Airline) || string.IsNullOrWhiteSpace(ticket.TNum) || ticket.NetPrice <= 0)
            {
                return false;
            }
        }
        return true;
    }
    public bool checkTicketInput(Ticket tickets)
    {
        if (string.IsNullOrWhiteSpace(ticket.PNR) || string.IsNullOrWhiteSpace(ticket.Airline) || string.IsNullOrWhiteSpace(ticket.TNum) || ticket.NetPrice <= 0)
        {
            return false;
        }
        return true;
    }
    private CreationStatus AddTickets(List<Ticket> tickets)
    {
        bool check = checkTicketInput(tickets);
        if (!check)
            return CreationStatus.InputError;
        if (db.Tickets.Any(t => t.PNR == tickets[0].PNR))
        {
            return CreationStatus.AlreadyExists;
        }
        db.Tickets.AddRange(tickets);
        return CreationStatus.Success;
    }
    private (char, CreationStatus) SaveTicketsToDB(EnginOutput output)
    {
        CreationStatus newTicketsStatus = AddTickets(output.newTickets);
        if (newTicketsStatus != CreationStatus.Success)
            return ('n', newTicketsStatus);
        CreationStatus oldTicketsStatus = AddTickets(output.oldTickets);
        if (oldTicketsStatus != CreationStatus.Success)
            return ('o', oldTicketsStatus);
        try
        {
            db.SaveChanges();
            return ('d', CreationStatus.Success);
        }
        catch (Exception)
        {
            return ('d', CreationStatus.Failure);
        }
    }
    public (char, CreationStatus) SellTicket(Employee employee, Client client, IssueCompany issueCompany, EnginOutput enginOutput)
    {
        //buisness logic
        // > on selling for customer
        //      employee.balance -=  sell;
        //      customer.balance -= sell;
        //      issueCompany.balance -= net;
        //      balance.virtualProfit += sell - net
        //      save ticket to db
        decimal sell = enginOutput.newTickets.Sum(t => t.SellPrice) + enginOutput.oldTickets.Sum(t => t.SellPrice);
        decimal net = enginOutput.newTickets.Sum(t => t.NetPrice) + enginOutput.oldTickets.Sum(t => t.NetPrice);
        employee.Balance -= sell;
        client.balance -= sell;
        issueCompany.Balance -= net;
        balance.VirtualProfit = sell - net;
        //update DB
        db.Update(employee);
        db.Update(client);
        db.Update(issueCompany);
        db.Update(balance);
        return SaveTicketsToDB(enginOutput);
    }
    public PaymentStatus CashPayment(List<Ticket> tickets, decimal amount)
    {
        List<Payment> payments = new();
        decimal TSellAmount = tickets.Sum(t => t.SellPrice);
        if (TSellAmount - amount != 0)
            return PaymentStatus.AmountError;
        //balance.value += sell
        balance.Value += amount;
        foreach (Ticket ticket in tickets)
        {
            Payment payment = new()
            {
                Amount = ticket.SellPrice,
                PaymentMethod = Cash,
                Status = PaymentStatus.Completed,
                Ticket = ticket,
                TransactionDate = DateTime.Now,
                TransactionId = $"CASH:{ticket.Id}"
            };
            // > on customer pay cash
            //     employee.balance += sell;
            //     customer.balance += sell;
            //     could be many clients paid
            ticket.Employee.Balance += ticket.SellPrice;
            ticket.Client.balance += ticket.SellPrice;
            payments.Add(payment);
            ticket.Payments.Add(payment);
        }
        db.Payments.AddRange(payments);
        db.Update(balance);
        db.UpdateRange(tickets);
        db.SaveChanges();
        return PaymentStatus.Completed;
    }
}

