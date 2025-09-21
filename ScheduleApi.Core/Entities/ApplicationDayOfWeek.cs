namespace ScheduleApi.Core.Entities;

public class ApplicationDayOfWeek
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Abbreviation { get; set; }

    public ICollection<Schedule> Schedules { get; set; }
    public ICollection<ScheduleOverride> ScheduleOverrides { get; set; }
}