using ScheduleApi.Application.DTOs.ScheduleOverride;

namespace ScheduleApi.Application.DTOs.Schedule;

public class ScheduleDto
{
    public int Id { get; set; }
    public string DayOfWeekName { get; set; }
    
    public int PairNumber { get; set; }
    public TimeOnly PairStartTime { get; set; }
    public TimeOnly PairEndTime { get; set; }
    
    public string GroupName { get; set; }
    public string TeacherFullName { get; set; }
    public string SubjectName { get; set; }
    public string SubjectTypeName { get; set; }
    
    public bool IsEvenWeek { get; set; }
    public int SemesterId { get; set; }
    public ScheduleOverrideInfoDto? OverrideInfo { get; set; }
}