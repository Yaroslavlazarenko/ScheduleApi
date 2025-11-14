namespace ScheduleApi.Application.DTOs.Schedule;

public record ScheduleOverrideFilterDto(DateTime? StartDate, DateTime? EndDate, int? GroupId, string TimeZoneId);