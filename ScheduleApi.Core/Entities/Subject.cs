namespace ScheduleApi.Core.Entities;

public class Subject
{
    public int Id { get; set; }

    public int SubjectTypeId { get; set; }
    public int SubjectNameId { get; set; }
    
    public SubjectType SubjectType { get; set; }
    public SubjectName SubjectName { get; set; }
    
    public ICollection<TeacherSubject> TeacherSubjects { get; set; }
    public ICollection<SubjectInfo> SubjectInfos { get; set; }
    public ICollection<GroupSubject> GroupSubjects { get; set; }
    public ICollection<Schedule> Schedules { get; set; }
}