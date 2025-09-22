using ScheduleApi.Application.DTOs.SubjectInfo;
using ScheduleApi.Application.DTOs.SubjectType;
using ScheduleApi.Application.DTOs.Teacher;

namespace ScheduleApi.Application.DTOs.Subject;

public class SubjectVariantDto
{
    public int Id { get; set; } 
    public SubjectTypeDto SubjectType { get; set; }
    public List<TeacherDto> Teachers { get; set; } = new(); 
    public List<SubjectInfoDto> Infos { get; set; } = new();
}