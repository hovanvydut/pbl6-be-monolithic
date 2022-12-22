namespace Monolithic.Extensions;

public static class DateTimeExtension
{
    private const string TIME_ZONE = "SE Asia Standard Time";

    public static DateTime GetLocalTime(this DateTime dateTime)
    {
        return TimeZoneInfo.ConvertTimeBySystemTimeZoneId(dateTime, TIME_ZONE);
    }
}