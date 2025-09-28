namespace ScheduleApi.Core.Entities;

public class GroupSubject
{
    public int Id { get; set; }
    public int GroupId { get; set; }
    public int TeacherSubjectId { get; set; }
    public int SemesterId { get; set; }

    public Group Group { get; set; }
    public TeacherSubject TeacherSubject { get; set; }
    public Semester Semester { get; set; }
    
    public ICollection<Schedule> Schedules { get; set; }
}