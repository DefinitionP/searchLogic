namespace searchLogic;

public class Time
{
    public int Days { get; set; }
    public int Hours { get; set; }
    public int Minutes { get; set; }
    public Time(int days, int hours, int minutes)
    {
        Days = days;
        Hours = hours;
        Minutes = minutes;
    }
}
