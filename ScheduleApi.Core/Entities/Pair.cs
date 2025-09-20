namespace ScheduleApi.Core.Entities;

public class Pair
{
    public int Id { get; set; }
    public int Number { get; set; }
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }

    public ICollection<Schedule> Schedules { get; set; }
}