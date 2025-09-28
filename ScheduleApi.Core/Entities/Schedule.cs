namespace ScheduleApi.Core.Entities;

public class Schedule
{
    public int Id { get; set; }
    public int ApplicationDayOfWeekId { get; set; }
    public int PairId { get; set; }
    public bool IsEvenWeek { get; set; }
    public int GroupSubjectId { get; set; }

    public ApplicationDayOfWeek ApplicationDayOfWeek { get; set; }
    public Pair Pair { get; set; }
    public GroupSubject GroupSubject { get; set; }
}