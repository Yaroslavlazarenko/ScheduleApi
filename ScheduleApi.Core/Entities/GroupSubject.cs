namespace ScheduleApi.Core.Entities;

public class GroupSubject
{
    public int GroupId { get; set; }
    public int TeacherId { get; set; }
    public int SubjectId { get; set; }

    public Group Group { get; set; }
    public Teacher Teacher { get; set; }
    public Subject Subject { get; set; }
    public TeacherSubject TeacherSubject { get; set; }
}