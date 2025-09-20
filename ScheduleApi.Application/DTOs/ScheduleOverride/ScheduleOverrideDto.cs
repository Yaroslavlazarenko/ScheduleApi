namespace ScheduleApi.Application.DTOs.ScheduleOverride;

public class ScheduleOverrideDto
{
    public int Id { get; set; }
    public DateTime OverrideDate { get; set; }
    
    public int OverrideTypeId { get; set; }
    public string OverrideTypeName { get; set; }
    
    public int? SubstituteDayOfWeekId { get; set; }
    public string? SubstituteDayOfWeekName { get; set; }
    
    public int? GroupId { get; set; }
    public string? GroupName { get; set; }
    
    public string? Description { get; set; }
}