using MediatR;
using Microsoft.AspNetCore.Mvc;
using ScheduleApi.Application.DTOs.Notification;
using ScheduleApi.Application.Features.Notification.Commands;
using ScheduleApi.Application.Features.Notification.Queries;

namespace ScheduleApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class NotificationController : ControllerBase
{
    private readonly IMediator _mediator;

    public NotificationController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpGet("pending-broadcast")]
    [ProducesResponseType(typeof(PendingBroadcastDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> GetPendingBroadcast()
    {
        var result = await _mediator.Send(new GetPendingBroadcast.Query());
        if (result == null)
        {
            return NoContent();
        }
        return Ok(result);
    }

    [HttpPost("broadcast/{id:int}/sent")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> MarkAsSent(int id)
    {
        await _mediator.Send(new MarkBroadcastAsSent.Command(id));
        return NoContent();
    }
}