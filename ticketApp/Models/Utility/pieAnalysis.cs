using System;

namespace ticketApp.Models.Utility;

public class pieAnalysis
{
    public string name { get; set; }
    public double percentage { get; set; }//must be a float number 0.25 means 25%
    public DateTime from { get; set; }
    public DateTime to { get; set; }

}
