using MediatR;
using Microsoft.AspNetCore.Mvc;
using ScheduleApi.Application.DTOs.ScheduleOverride;
using ScheduleApi.Application.Features.ScheduleOverride.Commands;
using ScheduleApi.Application.Features.ScheduleOverride.Queries;

namespace ScheduleApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ScheduleOverrideController : ControllerBase
{
    private readonly IMediator _mediator;

    public ScheduleOverrideController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CreateScheduleOverride([FromBody] MutateScheduleOverrideDto createDto)
    {
        var newId = await _mediator.Send(new CreateScheduleOverride.Command(createDto));
        return CreatedAtAction(nameof(GetScheduleOverrideById), new { id = newId }, new { id = newId });
    }

    [HttpGet]
    [ProducesResponseType(typeof(List<ScheduleOverrideDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetScheduleOverrides([FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate, [FromQuery] int? groupId)
    {
        var result = await _mediator.Send(new GetScheduleOverridesList.Query(startDate, endDate, groupId));
        return Ok(result);
    }
    
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(ScheduleOverrideDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetScheduleOverrideById(int id)
    {
        var result = await _mediator.Send(new GetScheduleOverrideById.Query(id));
        return Ok(result);
    }
    
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateScheduleOverride(int id, [FromBody] MutateScheduleOverrideDto updateDto)
    {
        await _mediator.Send(new UpdateScheduleOverride.Command(id, updateDto));
        return NoContent();
    }
    
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteScheduleOverride(int id)
    {
        await _mediator.Send(new DeleteScheduleOverride.Command(id));
        return NoContent();
    }
}