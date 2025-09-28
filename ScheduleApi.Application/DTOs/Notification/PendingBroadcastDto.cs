namespace ScheduleApi.Application.DTOs.Notification;

public class PendingBroadcastDto
{
    public int Id { get; set; }
    public string MessageText { get; set; }
    public List<UserBroadcastTargetDto> Users { get; set; }
}