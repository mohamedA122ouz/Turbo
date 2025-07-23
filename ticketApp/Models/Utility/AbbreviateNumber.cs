namespace ticketApp.Models.Utility;

public static class AbbreviateNumber
{
    public static string ToAbbreviateNumber(this decimal number)
    {
        decimal snumber = 1;
        if (number < 0)
        {
            snumber = -1;
            number *= -1;
        }
        if (number >= 1_000_000_000)
            return (number * snumber / 1_000_000_000M).ToString("0.#") + "B";
        else if (number >= 1_000_000)
            return (number * snumber / 1_000_000M).ToString("0.#") + "M";
        else if (number >= 1_000)
            return (number * snumber / 1_000M).ToString("0.#") + "K";
        else
            return number.ToString();
    }
    public static string ToAbbreviateNumber(this int number)
    {
        int snumber = 1;
        if (number < 0)
        {
            snumber = -1;
            number *= -1;
        }
        if (number >= 1_000_000_000)
            return (number * snumber / 1_000_000_000M).ToString("0.#") + "B";
        else if (number >= 1_000_000)
            return (number * snumber / 1_000_000M).ToString("0.#") + "M";
        else if (number >= 1_000)
            return (number * snumber / 1_000M).ToString("0.#") + "K";
        else
            return number.ToString();
    }
}
