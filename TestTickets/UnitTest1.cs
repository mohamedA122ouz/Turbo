using TestTickets.test;
using ticketApp.Models.DBmodels;
using ticketApp.Models.Utility;
using ticketApp.Services;

namespace TestTickets;


public class UnitTest1:TicketEngine
{
    public UnitTest1(DBContext db) : base(db) { }

    private string testPath = @"E:\git repo\ticket app\TestTickets\test";
    private string input(string? testFile = null) {
        string path = Path.Combine(testPath, "destinationError.txt");
        if(testFile != null)
            path = Path.Combine(testPath, testFile);
        FileStream file = new(path, FileMode.Open, FileAccess.Read);
        StreamReader fileReader = new(file);
        string inputText = fileReader.ReadToEnd();
        return inputText;
    }
    [Fact]
    public void TestERecodrd()
    {
        string inputText = input();
        Intializer(inputText);
        List<string> manualFares = [
            "MO/YAOBIN MR-/1576629144262/-EGP/43432.80/ET",
            "CAI/LIANGHONG MR-/1576629144263/-EGP/43432.80/ET",
            "DENG/RENHUA MR-/1576629144264/-EGP/43432.80/ET",
            "LIU/JINLIAN MS-/1576629144265/-EGP/43432.80/ET"
        ];
        int i = 0;
        try
        {
            foreach (var fare in manualFares)
            {
                Assert.True(fare == ERecord[i++]);
            }
        }
        catch
        {
            Assert.True(false);
        }
    }
    [Fact]
    public void TestflightDetails()
    {
        string inputText = input();
        Intializer(inputText);
        string output = "1 . QR 1304 M  04MAR CAIDOH HK4";
        try
        {
            Assert.True(flightDetails == output);
        }
        catch
        {
            Assert.True(false);
        }
    }
    [Fact]
    public void TestPNR() {
        string inputText = input();
        Intializer(inputText);
        string output = "738X9N";
        try {
            Assert.True(PNR == output);
        }
        catch {
            Assert.True(false);
        }
    }
    [Fact]
    public void TestOldTNum() {
        string inputText = input("reissue.txt");
        Intializer(inputText);
        List<string> output =
            [
                "0776628941219"
            ];

        try {
            int i = 0;
            foreach(var t in oldTicketNumbers) {
                Assert.True( t == output[i++]);
            }
            if (oldTicketNumbers.Count == 0) {
                Assert.True(false);
            }
        }
        catch {
            Assert.True(false);
        }
    }
    [Fact]
    public void TestCurrency() {
        string inputText = input("reissue.txt");
        Intializer(inputText);
        string currency_ = "EGP";

        try {
            Assert.True(currency == currency_);
        }
        catch {
            Assert.True(false);
        }
    }
    [Fact]
    public void TestTicketType() {
        string refund = input("refund2.txt");
        string voided = input("void.txt");
        string ticket = input("ticket1.txt");
        string reissue = input("reissue.txt");
        try {
            Intializer(refund);
            Assert.True(type == TicketType.Refund);
            Intializer(voided);
            Assert.True(type == TicketType.Void);
            Intializer(ticket);
            Assert.True(type == TicketType.Ticket);
            Intializer(reissue);
            Assert.True(type == TicketType.Reissue);
        }
        catch {
            Assert.True(false);
        }
    }
    [Fact]
    public void TestNetPrice() {
        string ticket = input("ticket6.txt");
        try {
            Intializer(ticket);
            List<decimal> netPrice_ = [
                (decimal)6326.80,
                (decimal)6326.80,
                (decimal)6326.80,
                (decimal)6326.80,
                (decimal)6326.80,
                (decimal)5524.80
                ];
            int i = 0;
            foreach (decimal netPrice in NetPrices) { 
                Assert.True(netPrice == netPrice_[i++]);
            }
        }
        catch {
            Assert.True(false);
        }
    }
    // [Theory]
    // [MemberData(nameof(TestCasesReader.Files), MemberType = typeof(TestCasesReader))]
    // public void Should_Pass_File_Test(NamedTestCase testCase)
    // {
    //     // Arrange
    //     var input = testCase.Data.Input;
    //     var expected = testCase.Data.Expected;
    //     Employee emp = db.Employees.FirstOrDefault(i => i.Email == "test@gmail.com");
    //     Client c = new()
    //     {
    //         Name = "testClient",
    //         Id = 3,
    //         PhoneNumber = "01112341234",
    //     };
    //     IssueCompany iata = new()
    //     {
    //         Name = "iata",
    //         Balance = 0
    //     };
    //     (CreationStatus, List<Ticket>) output = s.CreateTicketAutomatically(input, emp, null, c, iata);

    //     // Assert
    //     // Assert.Equal(expected, actual);
    // }

}