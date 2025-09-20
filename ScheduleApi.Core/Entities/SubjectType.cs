namespace ScheduleApi.Core.Entities;

public class SubjectType
{
    public int Id { get; set; }
    public string Name { get; set; }

    public ICollection<Subject> Subjects { get; set; }
}