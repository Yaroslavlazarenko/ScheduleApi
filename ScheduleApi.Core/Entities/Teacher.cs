namespace ScheduleApi.Core.Entities;

public class Teacher
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string? MiddleName { get; set; }

    public ICollection<TeacherSubject> TeacherSubjects { get; set; }
    public ICollection<TeacherInfo> TeacherInfos { get; set; }
}