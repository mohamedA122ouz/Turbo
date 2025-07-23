namespace ticketApp.Models.DBmodels {
    public class Balance
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public decimal Value { get; set; }
        //the company should have this amount of the profit
        public decimal VirtualProfit { get; set; }
    }
}
