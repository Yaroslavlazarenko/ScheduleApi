using ScheduleApi.Application.DTOs.SubjectInfo;

namespace ScheduleApi.Application.DTOs.Subject;

public class SubjectDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    
    public string SubjectTypeName { get; set; } 
    public List<SubjectInfoDto> Infos { get; set; }
}