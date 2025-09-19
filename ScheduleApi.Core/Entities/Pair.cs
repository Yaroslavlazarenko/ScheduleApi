namespace ScheduleApi.Core.Entities;

public class Pair
{
    public int Id { get; set; }
    public int Number { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }

    public ICollection<Schedule> Schedules { get; set; }
}