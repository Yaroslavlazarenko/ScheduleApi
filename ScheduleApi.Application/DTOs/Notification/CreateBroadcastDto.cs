using System.ComponentModel.DataAnnotations;

namespace ScheduleApi.Application.DTOs.Notification;

public class CreateBroadcastDto
{
    [Required]
    [MinLength(1)]
    [MaxLength(4000)]
    public string MessageText { get; set; }

    public DateTime? ScheduledAt { get; set; }
}