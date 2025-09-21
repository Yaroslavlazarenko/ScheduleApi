using ScheduleApi.Application.DTOs.SubjectInfo;
using ScheduleApi.Application.DTOs.SubjectType;
using ScheduleApi.Application.DTOs.Teacher;

namespace ScheduleApi.Application.DTOs.Subject;

public class SubjectDetailsDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string ShortName { get; set; }
    public string Abbreviation { get; set; }
    
    public SubjectTypeDto SubjectType { get; set; } 
    public List<SubjectInfoDto> Infos { get; set; }
    
    public List<SubjectTeacherDto> Teachers { get; set; } = new();
}