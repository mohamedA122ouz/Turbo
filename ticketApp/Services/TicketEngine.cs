using Microsoft.VisualStudio.TextTemplating;
using System.Text.RegularExpressions;

using ticketApp.Models.DBmodels;
using ticketApp.Models.Utility;
namespace ticketApp.Services;

public class RegexManger//will be moved to utilities folder
{
    protected string ERecordRegex = @"(\w+(?:\s\w+)*)(/|\s)(\w+(?:\s\w+)*)(\s\w+)?-/\d{13}/-(EGP|USD|SAR)/\d+\.\d{2}/ET(\s/\w+)?";
    protected string ticketcountRegex = @"\W(HK|TK|HX)\d+";
    protected string MonthsRegex = @"JAN|FEB|MAR|APR|MAY|JUN|JUL|AUG|SEP|OCT|NOV|DES";
    protected string PNRLineRegex = @"[0-9 | A-Z]{6}\/\w{2}";
    //Fare is the netPrice in most cases
    protected string FareRegex = @"TOT\sEGP(\d|\W)+ADDITIONAL";
    protected string flighDetailsRegex = @"\d\W+\w{2}\W(.*)\d{2}(JAN|FEB|MAR|APR|MAY|JUN|JUL|AUG|SEP|OCT|NOV|DES)\W\w{6}\W(HK|TK|HX)\d+";
    protected string VoidRegex = @"/VOID";
    protected string RefundRegex = @"RFND";
    protected string TicketRegex = @"NO HISTORY TIN DATA";
    protected string OldTicketPart = @"\*\sHISTORY\sTIN\sDATA\s\*(\w|\W)*\*\sC";
    protected string NewTicketPart = @"\*\sCurrent\sTIN\sDATA\s\*(\w|\W)*\*\sET";
    protected string currencyRegex = @"(EGP|USD|SAR)";
    protected string ClientName = @"P\d+\s+(\w+(?:\s\w+)*)(/|\s)(\w+(?:\s\w+)*)(\w+)?";
}
public enum TicketType
{
    Reissue,
    Void,
    Refund,
    Ticket
}
public class TicketEngine : RegexManger
{
    protected TicketType type { get; set; }
    private List<string> oldTnums { get; set; }
    public List<string> oldTicketNumbers { get => oldTnums; }
    private List<string> Tnums { get; set; }
    public List<string> TicketNumbers { get => Tnums; }
    private string destination { get; set; }
    protected int count { get; set; } = 0;
    protected List<string> ERecord { get; set; }
    protected string flightDetails { get; set; }
    public List<decimal> SellPcrices { get; set; }
    protected string PNR { get; set; }
    protected List<decimal> NetPrices { get; set; }
    private string _currency { get; set; }
    public string currency { get => _currency; }
    private List<decimal> _FarePrice { get; set; } = new();
    public List<decimal> FarePrice { get => _FarePrice; }
    protected string airline { get; set; }
    private bool initialized { get; set; } = false;
    //need to extract destination
    //need to extract 
    /*private */
    private DBContext db { get; set; }
    public TicketEngine(DBContext db)
    {
        this.db = db;
    }

    public void Intializer(string input)
    {
        SpecifyTicketType(input);
        extractERecord(input);
        extractFlightDetails(input);
        if (type == TicketType.Reissue)
            extractOldTicket(input);
        extractPNR(input);
        extractFar(input);
        extractTicketNums();
        extractAirline();
        extractNetPrice();
        extractCurrency();
        extractDestination();
        initialized = true;
    }
    protected void extractERecord(string input)
    {
        /* 
        - Example of ERecord
            ALY/MARAM-/0656628946251/-EGP/5294.80/ET
            AHMED/EMAN MS-/0656628946252/-EGP/5942.80/ET
            ALI/YOUSEF-/0656628946253/-EGP/5294.80/ET
        */
        if (count == 0)
            extractCount(input);
        Regex regex = new(ERecordRegex, RegexOptions.Multiline);
        ERecord = regex.Matches(input).Take(type == TicketType.Reissue ? count * 2 : count).Select(m => m.Value).ToList();
    }
    protected void extractCount(string input)
    {
        if (type == TicketType.Refund)
        {
            Regex regex1 = new(ERecordRegex, RegexOptions.Multiline);
            count = regex1.Matches(input).Count();
            return;
        }
        Regex regex = new(ticketcountRegex, RegexOptions.Multiline);
        Match countMatch = regex.Match(input);
        Regex number = new(@"\d+");
        count = int.Parse(number.Match(countMatch.Value).Value);
    }
    protected void extractFlightDetails(string input)
    {
        /*
        - Example of flight details
            1. SV  312 Q 06MAR CAIRUH HK3
            2. SV 1469 B 07MAR RUHMED HK3 
        */
        Regex regex = new(flighDetailsRegex, RegexOptions.Multiline);
        flightDetails = regex.Match(input).Value;
    }
    protected void extractPNR(string input)
    {
        Regex regex = new(PNRLineRegex);
        string PNRLine = regex.Match(input).Value;
        Regex PNRRegex = new(@"(\d|\w){6}");
        PNR = PNRRegex.Match(PNRLine).Value;
    }
    public void NetPriceAddedTo(List<decimal> Values)
    {
        SellPcrices = NetPrices.Select((el, i) => el + Values[i]).ToList();
    }
    public void NetPriceAddedTo(decimal Value)
    {
        SellPcrices = NetPrices.Select((el, i) => el + Value).ToList();
    }
    protected void extractNetPrice(decimal voidedValue = 0)
    {
        if (type == TicketType.Void)
        {
            NetPrices = ERecord.Select(i => (decimal)voidedValue).ToList();
        }
        Regex price = new(@"\d+\.\d{2}");
        NetPrices = ERecord.Select(el => decimal.Parse(price.Match(el).Value)).ToList();
    }
    protected void SpecifyTicketType(string input)
    {
        Regex voided = new(VoidRegex);
        Match isVoided = voided.Match(input);
        if (isVoided.Length != 0)
        {
            type = TicketType.Void;
            return;
        }

        Regex refund = new(RefundRegex);
        Match isRefund = refund.Match(input);
        if (isRefund.Length != 0)
        {
            type = TicketType.Refund;
            return;
        }

        Regex ticket = new(TicketRegex);
        Match isticket = ticket.Match(input);
        if (isticket.Length != 0)
        {
            type = TicketType.Ticket;
            return;
        }
        type = TicketType.Reissue;
    }
    private void extractOldTicket(string input)
    {
        /*
        - Example of old ticket part

            * HISTORY TIN DATA *
            XK NOFAL/ADNAN ISMA-/0776628941219/-EGP/4490.00/ET
            * C

        */
        if (string.IsNullOrEmpty(input)) { return; }
        else if (type != TicketType.Reissue)
        {
            return;
        }
        Regex regex = new(OldTicketPart);
        string historyPart = regex.Match(input).Value;
        Regex TicketNumbers = new(@"\d{13}");
        oldTnums = TicketNumbers.Matches(historyPart).Select(el => el.Value).ToList();
    }
    private void extractTicketNums()
    {
        Regex TicketNumbers = new(@"\d{13}");
        int skipCount = 0;
        if (type == TicketType.Reissue)
            skipCount = count;
        Tnums = ERecord.Skip(skipCount).Select(el => TicketNumbers.Match(el).Value).ToList();
    }
    private void extractCurrency()
    {
        Regex regex = new(currencyRegex);
        _currency = regex.Match(ERecord[0]).Value;
    }
    private void extractFar(string input)
    {
        if (type == TicketType.Refund)
            return;
        Regex regex = new(FareRegex, RegexOptions.Multiline);
        _FarePrice = regex.Matches(input).Select(el =>
        {
            string holder = el.Value.Replace("TOT EGP", "");
            holder = holder.Replace("***ADDITIONAL", "");
            holder = holder.Replace("\n", "");
            holder = holder.Replace("\r", "");
            return decimal.Parse(holder);
        }).ToList();
    }
    private void extractDestination()
    {
        Regex regex = new(@"\s\w{6}\s");
        destination = regex.Match(flightDetails).Value.Trim();
    }
    private void extractAirline()
    {
        if (type == TicketType.Refund)
        {
            //from database specify the refunded ticket and make a refund payment
            //although the void have the flightDetails but have 
            //the same concept of refund
            return;
        }
        Regex airlineRegex = new(@"\w\w");
        string FlightDetail = flightDetails;
        airline = airlineRegex.Match(FlightDetail).Value;
    }
    public void applyFareDiscount(string airline, decimal value)
    {
        if (this.airline != airline)
            return;
        if (value < 0)
            value *= -1;
        else if (value == 0)
            return;
        if (value > 1)//if percentage is in integer form
            value = (value % 101) / 100;//make sure it is [0-100] and then convert it to percentage by divide over 100
        decimal output = value * FarePrice[0];
        NetPrices = NetPrices.Select(price => price - output).ToList();
    }
    public EnginOutput? createTickets(Employee emp, IssueCompany issueCompany, Broker? broker = null, Client? client = null, decimal saleIncreaseValue = 0)
    {
        int i = 0;
        if (!initialized)
        {
            return null;
        }
        EnginOutput? output = new();
        if (client == null)
        {
            client = db.Clients.FirstOrDefault(c => c.Name == "Unknown")!;
        }
        if (SellPcrices == null)
            NetPriceAddedTo(saleIncreaseValue);
        if (type == TicketType.Reissue)
        {
            output.oldTickets = oldTnums.Select(
                (tNum) =>
                {
                    Ticket? t = db.Tickets.FirstOrDefault(t => t.TNum == tNum);
                    if (t != null)
                        return t;//Old Ticket if exist
                    //what if reissue for non exist ticket
                    output.isOldExistinDB = false;
                    return new Ticket()
                    {
                        Airline = airline,
                        Client = client,
                        Employee = emp,
                        Broker = broker,
                        IssueCompany = issueCompany,
                        //notice sell and net price for old tickets comes first
                        NetPrice = NetPrices[i],//must be global iterator to cause the netPrice is list of poth issue and reissue tickets net price
                        PNR = PNR,
                        isAReIssued = type == TicketType.Reissue,
                        TNum = oldTnums[i],
                        Destination = destination,
                        SellPrice = SellPcrices[i++],//must be global iterator to cause the sellPrice is list of poth issue and reissue tickets sell price
                        ClientId = client.Id,
                        EmployeeId = emp.Id
                    };
                }).ToList();
        }
        output.newTickets = TicketNumbers.Select((t, ii) =>
        {

            // decimal netNetPrice = NetPrices[i];//must use global iterator to cause the netPrice is list of poth issue and reissue tickets net price
            // decimal netSellPrice = SellPcrices[i++];//must be global iterator to cause the sellPrice is list of poth issue and reissue tickets sell price
            // //the iterator must be increasing at the last use to get ready for the next step
            // if (type == TicketType.Reissue)
            // {
            //     netNetPrice -= NetPrices[ii];//cause the new ticket must substract old ticket value (already paid part)
            //     netSellPrice -= SellPcrices[ii];//same thing here
            // }
            return new Ticket()
            {
                Airline = airline,
                Client = client,
                Employee = emp,
                Broker = broker,
                IssueCompany = issueCompany,
                NetPrice = NetPrices[i],//must use global iterator to cause the netPrice is list of poth issue and reissue tickets net price
                PNR = PNR,
                isAReIssued = type == TicketType.Reissue,
                TNum = t,
                Destination = destination,
                SellPrice = SellPcrices[i++],//must be global iterator to cause the sellPrice is list of poth issue and reissue tickets sell price
                ClientId = client.Id,
                EmployeeId = emp.Id,
            };
        }).ToList();
        return output;
    }
    public CreationStatus SaveTicketToDB(Ticket ticket)
    {
        db.Tickets.Add(ticket);
        ticket.Employee.Tickets.Add(ticket);
        db.Entry(ticket).Reference(t => t.IssueCompany).Load();
        if (ticket.Broker != null)
        {
            ticket.Broker.Tickets.Add(ticket);
            ticket.Broker.Balance -= ticket.NetPrice; // Broker's balance should be decreased by the ticket price
        }
        else
            ticket.Employee.Balance -= ticket.NetPrice;
        // Issued ticket should be subtracted from the employee's balance and the issue company's balance
        ticket.IssueCompany.Balance -= ticket.NetPrice;
        if (string.IsNullOrWhiteSpace(ticket.PNR) || string.IsNullOrWhiteSpace(ticket.Airline) || string.IsNullOrWhiteSpace(ticket.TNum) || ticket.NetPrice <= 0)
        {
            return CreationStatus.InputError;
        }
        if (db.Tickets.Any(t => t.PNR == ticket.PNR))
        {
            return CreationStatus.AlreadyExists;
        }

        try
        {
            db.SaveChanges();
            return CreationStatus.Success;
        }
        catch (Exception)
        {
            return CreationStatus.Failure;
        }
    }
    public CreationStatus NoRevisiounCreation(string ticketDetails, Employee emp, Broker? broker, Client client, IssueCompany issueCompany, Func<Ticket, CreationStatus, bool>? func = null)
    {

        if (string.IsNullOrWhiteSpace(ticketDetails))
        {
            return CreationStatus.InputError;
        }
        EnginOutput? EOutput = createTickets(emp, issueCompany, broker, client);
        if (EOutput == null)
            return CreationStatus.Failure;
        EnginTicketSave(EOutput, func);
        return CreationStatus.Success;
    }
    public (CreationStatus, EnginOutput?) EnginTicketSave(EnginOutput EOutput, Func<Ticket, CreationStatus, bool>? func = null)
    {
        if (type == TicketType.Reissue && EOutput.oldTickets.Count != EOutput.newTickets.Count)
        {
            return (CreationStatus.NotFound, EOutput);
        }
        foreach (Ticket t in EOutput.newTickets)
        {
            CreationStatus i = SaveTicketToDB(t);
            if (i != CreationStatus.Success)
            {
                if (func != null)
                    func(t, i);
                else
                {
                    if (i == CreationStatus.Failure)
                    {
                        return (CreationStatus.Failure, EOutput);
                    }
                    else if (i == CreationStatus.AlreadyExists)
                    {
                        return (CreationStatus.Failure, EOutput);
                    }
                }
            }
        }
        //only if reissue
        if (TicketType.Reissue == type)
        {
            int i = 0;
            foreach (Ticket t in EOutput.newTickets)
            {
                Ticket oldTicket = EOutput.oldTickets[i++];
                oldTicket.isAReIssued = true;
                db.ReIssuedTickets.Add(new()
                {
                    OldTnum = oldTicket.TNum,
                    NewTicket = t
                });
                db.Update(oldTicket);
            }
            db.SaveChanges();
        }
        return (CreationStatus.Success, EOutput);
    }
}
