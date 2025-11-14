using ScheduleApi.Core.Entities;

namespace ScheduleApi.Application.Services;

public interface IScheduleTimeContextService
{
    /// <summary>
    /// Создает временной контекст на основе времени пользователя и его часового пояса.
    /// </summary>
    /// <param name="targetDateTime">Запрошенное пользователем время. Если null, используется текущее время.</param>
    /// <param name="userTimeZoneId">IANA ID часового пояса пользователя (например, "Europe/Warsaw").</param>
    /// <returns>Объект ScheduleTimeContext с рассчитанными временными метками.</returns>
    ScheduleTimeContext CreateContext(DateTime? targetDateTime, string userTimeZoneId);
    
    /// <summary>
    /// Возвращает объект TimeZoneInfo для "авторитетного" часового пояса (Украина).
    /// </summary>
    /// <returns>Объект TimeZoneInfo.</returns>
    TimeZoneInfo GetAuthoritativeTimeZone();
    
    UtcDateRange CreateUtcDateRange(DateTime? startDate, DateTime? endDate, string timeZoneId);
}