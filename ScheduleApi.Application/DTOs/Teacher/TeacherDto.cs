using ScheduleApi.Application.DTOs.TeacherInfo;

namespace ScheduleApi.Application.DTOs.Teacher;

public class TeacherDto
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string? MiddleName { get; set; }
    public string FullName { get; set; }

    public List<TeacherInfoDto> Infos { get; set; } = new();
}