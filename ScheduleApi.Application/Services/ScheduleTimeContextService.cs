using ScheduleApi.Core.Entities;
using ScheduleApi.Core.Exceptions;

namespace ScheduleApi.Application.Services;

public class ScheduleTimeContextService : IScheduleTimeContextService
{
    private const string UkraineTimeZoneId = "Europe/Kyiv";
    private readonly TimeZoneInfo _ukraineTimeZone;

    public ScheduleTimeContextService()
    {
        try
        {
            _ukraineTimeZone = TimeZoneInfo.FindSystemTimeZoneById(UkraineTimeZoneId);
        }
        catch (TimeZoneNotFoundException)
        {
            throw new InvalidOperationException($"The authoritative time zone '{UkraineTimeZoneId}' was not found on the system.");
        }
    }

    public ScheduleTimeContext CreateContext(DateTime? targetDateTime, string userTimeZoneId)
    {
        var userTimeZone = FindTimeZone(userTimeZoneId);

        var targetTimeUtc = targetDateTime.HasValue
            ? TimeZoneInfo.ConvertTimeToUtc(DateTime.SpecifyKind(targetDateTime.Value, DateTimeKind.Unspecified), userTimeZone)
            : DateTime.UtcNow;

        var authoritativeTime = TimeZoneInfo.ConvertTimeFromUtc(targetTimeUtc, _ukraineTimeZone);
        
        var startOfDayUkraine = authoritativeTime.Date;
        var endOfDayUkraine = startOfDayUkraine.AddDays(1);

        var startOfDayUtc = TimeZoneInfo.ConvertTimeToUtc(startOfDayUkraine, _ukraineTimeZone);
        var endOfDayUtc = TimeZoneInfo.ConvertTimeToUtc(endOfDayUkraine, _ukraineTimeZone);

        return new ScheduleTimeContext
        {
            AuthoritativeTime = authoritativeTime,
            StartOfDayUtc = startOfDayUtc,
            EndOfDayUtc = endOfDayUtc
        };
    }
    
    public UtcDateRange CreateUtcDateRange(DateTime? startDate, DateTime? endDate, string timeZoneId)
    {
        if (startDate is null && endDate is null)
        {
            return new UtcDateRange(null, null);
        }
        
        var timeZone = FindTimeZone(timeZoneId); // Переиспользуем наш приватный метод!

        DateTime? startDateUtc = null;
        if (startDate.HasValue)
        {
            // Берем начало дня и конвертируем в UTC
            startDateUtc = TimeZoneInfo.ConvertTimeToUtc(startDate.Value.Date, timeZone);
        }

        DateTime? endDateUtc = null;
        if (endDate.HasValue)
        {
            // Берем начало СЛЕДУЮЩЕГО дня для создания эксклюзивной верхней границы
            var nextDay = endDate.Value.Date.AddDays(1);
            endDateUtc = TimeZoneInfo.ConvertTimeToUtc(nextDay, timeZone);
        }

        return new UtcDateRange(startDateUtc, endDateUtc);
    }
    
    public TimeZoneInfo GetAuthoritativeTimeZone()
    {
        return _ukraineTimeZone;
    }

    private TimeZoneInfo FindTimeZone(string timeZoneId)
    {
        try
        {
            return TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
        }
        catch (TimeZoneNotFoundException)
        {
            throw new BadRequestException($"The specified time zone '{timeZoneId}' is invalid.");
        }
    }
}