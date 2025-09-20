using ScheduleApi.Application.DTOs.SubjectInfo;
using ScheduleApi.Application.DTOs.SubjectType;

namespace ScheduleApi.Application.DTOs.Subject;

public class SubjectDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string ShortName { get; set; }
    public string Abbreviation { get; set; }
    
    public SubjectTypeDto SubjectType { get; set; } 
    public List<SubjectInfoDto> Infos { get; set; }
}