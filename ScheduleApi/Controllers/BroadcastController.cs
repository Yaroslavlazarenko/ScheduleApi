using MediatR;
using Microsoft.AspNetCore.Mvc;
using ScheduleApi.Application.DTOs.Notification;
using ScheduleApi.Application.Features.Notification.Commands;
using ScheduleApi.Security;

namespace ScheduleApi.Controllers;

[ApiKeyAuthorize]
[ApiController]
[Route("api/[controller]")]
public class BroadcastController : ControllerBase
{
    private readonly IMediator _mediator;
    public BroadcastController(IMediator mediator) => _mediator = mediator;

    [HttpPost]
    [ProducesResponseType(typeof(object), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateBroadcast([FromBody] CreateBroadcastDto createDto)
    {
        var command = new CreateBroadcast.Command(createDto);
        var broadcastId = await _mediator.Send(command);
        return CreatedAtAction(nameof(CreateBroadcast), new { id = broadcastId }, new { id = broadcastId });
    }
}