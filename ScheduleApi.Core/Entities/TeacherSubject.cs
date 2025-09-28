namespace ScheduleApi.Core.Entities;

public class TeacherSubject
{
    public int Id { get; set; }
    public int TeacherId { get; set; }
    public int SubjectId { get; set; }
    
    public string? LessonUrl { get; set; }
    public int? SocialMediaTypesId { get; set; }
    
    public Teacher Teacher { get; set; }
    public Subject Subject { get; set; }
    public SocialMediaType SocialMediaType { get; set; }
    
    public ICollection<GroupSubject> GroupSubjects { get; set; }
}