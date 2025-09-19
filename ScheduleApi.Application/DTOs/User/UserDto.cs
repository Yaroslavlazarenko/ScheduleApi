namespace ScheduleApi.Application.DTOs.User;

public class UserDto
{
    public int Id { get; set; } 
    public long? TelegramId { get; set; } 
    public int GroupId { get; set; }
    public int RegionId { get; set; }
    public bool IsAdmin { get; set; }
}