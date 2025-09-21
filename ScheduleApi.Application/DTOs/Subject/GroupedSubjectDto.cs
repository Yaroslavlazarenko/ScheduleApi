namespace ScheduleApi.Application.DTOs.Subject;

public class GroupedSubjectDto
{
    public string Name { get; set; }
    public string ShortName { get; set; }
    public string Abbreviation { get; set; }
    
    public List<SubjectVariantDto> Variants { get; set; } = new();
}