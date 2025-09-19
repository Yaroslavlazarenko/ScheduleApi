namespace ScheduleApi.Core.Entities;

public class Region
{
    public int Id { get; set; }
    public int Number { get; set; }

    public ICollection<User> Students { get; set; }
}