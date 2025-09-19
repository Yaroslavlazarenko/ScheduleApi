namespace ScheduleApi.Core.Entities;

public class ScheduleOverride
{
    public int Id { get; set; }
    public DateTime OverrideDate { get; set; }
    public int OverrideTypeId { get; set; }
    public int? SubstituteDayOfWeekId { get; set; }
    public int? GroupId { get; set; }
    public string Description { get; set; }

    public OverrideType OverrideType { get; set; }
    public ApplicationDayOfWeek SubstituteDayOfWeek { get; set; }
    public Group Group { get; set; }
}