using ScheduleApi.Application.DTOs.ScheduleOverride;

namespace ScheduleApi.Application.DTOs.Schedule;

public class DailyScheduleDto
{
    public DateOnly Date { get; set; }
    public string DayOfWeekName { get; set; }
    public string DayOfWeekAbbreviation { get; set; }
    public int WeekNumber { get; set; }
    public bool IsEvenWeek { get; set; }
    
    public ScheduleOverrideInfoDto? OverrideInfo { get; set; }
    
    public List<LessonDto> Lessons { get; set; }
}