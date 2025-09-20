namespace ScheduleApi.Application.DTOs.Pair;

public class PairDto
{
    public int Id { get; set; }
    public int Number { get; set; }
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }
}