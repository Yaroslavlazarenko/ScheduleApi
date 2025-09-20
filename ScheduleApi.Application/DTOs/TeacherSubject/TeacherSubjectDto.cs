namespace ScheduleApi.Application.DTOs.TeacherSubject;

public class TeacherSubjectDto
{
    public int SubjectId { get; set; }
    public string SubjectName { get; set; }
    
    public string? LessonUrl { get; set; }
    
    public int? SocialMediaTypesId { get; set; }
    public string? SocialMediaTypeName { get; set; }
}