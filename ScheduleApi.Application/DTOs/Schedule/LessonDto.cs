namespace ScheduleApi.Application.DTOs.Schedule;

public class LessonDto
{
    public int PairNumber { get; set; }
    public TimeOnly PairStartTime { get; set; }
    public TimeOnly PairEndTime { get; set; }

    public string SubjectName { get; set; }
    public string SubjectShortName { get; set; } 
    public string SubjectAbbreviation { get; set; }
    public string SubjectTypeAbbreviation { get; set; }
    public string TeacherFullName { get; set; }
    public string? LessonUrl { get; set; }
}