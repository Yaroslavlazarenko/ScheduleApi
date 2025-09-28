namespace ScheduleApi.Core.Entities;

public class Broadcast
{
    public int Id { get; set; }
    public string MessageText { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public DateTime ScheduledAt { get; set; }
    
    public DateTime? SentAt { get; set; }
}