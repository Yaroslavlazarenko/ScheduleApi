namespace ScheduleApi.Application.DTOs.Subject;

public class CreateSubjectDto
{
    public string Name { get; set; }
    public string ShortName { get; set; }
    public string Abbreviation { get; set; }
    public int SubjectTypeId { get; set; }
}