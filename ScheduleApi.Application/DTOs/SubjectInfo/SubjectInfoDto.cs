namespace ScheduleApi.Application.DTOs.SubjectInfo;

public class SubjectInfoDto
{
    public int InfoTypeId { get; set; }
    public string InfoTypeName { get; set; }
    public string Value { get; set; }
    public string? Description { get; set; }
}