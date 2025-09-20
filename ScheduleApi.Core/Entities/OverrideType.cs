namespace ScheduleApi.Core.Entities;

public class OverrideType
{
    public int Id { get; set; }
    public string Name { get; set; }

    public ICollection<ScheduleOverride> ScheduleOverrides { get; set; }
}