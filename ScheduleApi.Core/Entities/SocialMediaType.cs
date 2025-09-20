namespace ScheduleApi.Core.Entities;

public class SocialMediaType
{
    public int Id { get; set; }
    public string Name { get; set; }

    public ICollection<TeacherSubject> TeacherSubjects { get; set; }
}