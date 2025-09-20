namespace ScheduleApi.Core.Entities;

public class Schedule
{
    public int Id { get; set; }
    public int ApplicationDayOfWeekId { get; set; }
    public int PairId { get; set; }
    public int GroupId { get; set; }
    public int TeacherId { get; set; }
    public int SubjectId { get; set; }
    public bool IsEvenWeek { get; set; }
    public int SemesterId { get; set; }
    public ApplicationDayOfWeek ApplicationDayOfWeek { get; set; }
    public Pair Pair { get; set; }
    public Group Group { get; set; }
    public Teacher Teacher { get; set; }
    public Subject Subject { get; set; }
    public Semester Semester { get; set; }
}