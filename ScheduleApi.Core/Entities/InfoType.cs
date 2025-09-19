namespace ScheduleApi.Core.Entities;

public class InfoType
{
    public int Id { get; set; }
    public string Name { get; set; }

    public ICollection<TeacherInfo> TeacherInfos { get; set; }
    public ICollection<SubjectInfo> SubjectInfos { get; set; }
}