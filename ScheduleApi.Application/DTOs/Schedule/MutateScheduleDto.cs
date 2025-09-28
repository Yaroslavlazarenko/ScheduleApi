namespace ScheduleApi.Application.DTOs.Schedule;

public class MutateScheduleDto
{
    public int ApplicationDayOfWeekId { get; set; }
    public int PairId { get; set; }
    public bool IsEvenWeek { get; set; }
    public int GroupSubjectId { get; set; }
}