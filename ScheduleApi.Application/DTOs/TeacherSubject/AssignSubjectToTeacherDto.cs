namespace ScheduleApi.Application.DTOs.TeacherSubject;

public class AssignSubjectToTeacherDto
{
    public int SubjectId { get; set; }

    public string? LessonUrl { get; set; }
    
    public int? SocialMediaTypesId { get; set; }
}