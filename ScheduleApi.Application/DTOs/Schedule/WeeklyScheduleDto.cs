namespace ScheduleApi.Application.DTOs.Schedule;

public class WeeklyScheduleDto
{
    public string GroupName { get; set; }
    public DateOnly WeekStartDate { get; set; }
    public DateOnly WeekEndDate { get; set; }
    public int WeekNumber { get; set; }
    public bool IsEvenWeek { get; set; }
    public string TimeZoneId { get; set; }
    public List<DailyScheduleDto> DailySchedules { get; set; }
}