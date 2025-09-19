namespace ScheduleApi.Core.Entities;

public class User
{
    public int Id { get; set; } 

    public long? TelegramId { get; set; } 
    public int GroupId { get; set; }
    public int RegionId { get; set; }
    public bool IsAdmin { get; set; }

    public Group Group { get; set; }
    public Region Region { get; set; }
}