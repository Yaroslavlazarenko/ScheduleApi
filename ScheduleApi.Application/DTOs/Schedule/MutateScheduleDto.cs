namespace ScheduleApi.Application.DTOs.Schedule;

public class MutateScheduleDto
{
    public int ApplicationDayOfWeekId { get; set; }

    public int PairId { get; set; }

    public int GroupId { get; set; }

    public int TeacherId { get; set; }

    public int SubjectId { get; set; }

    public bool IsEvenWeek { get; set; }

    public int SemesterId { get; set; }
}