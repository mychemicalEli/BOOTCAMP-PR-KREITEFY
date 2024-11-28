namespace api.Application;

public static class TimeSpanConverter
{
    public static string ToString(TimeSpan duration)
    {
        return $"{duration.Minutes:D2}:{duration.Seconds:D2}";
    }

    public static TimeSpan ToTimeSpan(string duration)
    {
        var parts = duration.Split(':');
        var minutes = int.Parse(parts[0]);
        var seconds = int.Parse(parts[1]);
        return new TimeSpan(0, minutes, seconds);
    }
}