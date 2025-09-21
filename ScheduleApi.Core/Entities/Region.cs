namespace ScheduleApi.Core.Entities;

public class Region
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string TimeZoneId { get; set; }

    public ICollection<User> Users { get; set; }
}