namespace ScheduleApi.Core.Entities;

public class Semester
{
    public int Id { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }

    public ICollection<Schedule> Schedules { get; set; }
}