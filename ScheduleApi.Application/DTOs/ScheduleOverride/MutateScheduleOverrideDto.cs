namespace ScheduleApi.Application.DTOs.ScheduleOverride;

public class MutateScheduleOverrideDto
{
    public DateTime OverrideDate { get; set; }

    public int OverrideTypeId { get; set; }
    
    public int? SubstituteDayOfWeekId { get; set; }
    public int? GroupId { get; set; }
    
    public string? Description { get; set; }
}