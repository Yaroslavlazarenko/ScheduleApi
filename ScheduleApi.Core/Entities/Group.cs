namespace ScheduleApi.Core.Entities;

public class Group
{
    public int Id { get; set; }
    public string Name { get; set; }

    public ICollection<User> Users { get; set; }
    public ICollection<GroupSubject> GroupSubjects { get; set; }
    public ICollection<Schedule> Schedules { get; set; }
    public ICollection<ScheduleOverride> ScheduleOverrides { get; set; }
}