namespace ScheduleApi.Core.Entities;

public class SubjectName
{
    public int Id { get; set; }
    public string FullName { get; set; }
    public string ShortName { get; set; }
    public string Abbreviation { get; set; }

    public ICollection<Subject> Subjects { get; set; } 
}