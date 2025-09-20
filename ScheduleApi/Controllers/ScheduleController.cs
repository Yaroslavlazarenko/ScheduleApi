using MediatR;
using Microsoft.AspNetCore.Mvc;
using ScheduleApi.Application.DTOs.Schedule;
using ScheduleApi.Application.Features.Schedule.Commands;
using ScheduleApi.Application.Features.Schedule.Queries;

namespace ScheduleApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ScheduleController : ControllerBase
{
    private readonly IMediator _mediator;

    public ScheduleController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("group")]
    [ProducesResponseType(typeof(DailyScheduleDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetGroupDailySchedule(
        [FromQuery] int groupId, 
        [FromQuery] string timeZoneId, 
        [FromQuery] DateTime? date)
    {
        var query = new GetGroupDailySchedule.Query(groupId, timeZoneId, date);
        var result = await _mediator.Send(query);
        return Ok(result);
    }
    
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CreateSchedule([FromBody] MutateScheduleDto createDto)
    {
        var command = new CreateSchedule.Command(createDto);
        var newId = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetScheduleById), new { id = newId }, new { id = newId });
    }

    [HttpGet]
    [ProducesResponseType(typeof(List<ScheduleDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetSchedules([FromQuery] int? groupId, [FromQuery] int? semesterId, [FromQuery] int? teacherId)
    {
        var query = new GetSchedulesList.Query(groupId, semesterId, teacherId);
        var result = await _mediator.Send(query);
        return Ok(result);
    }
    
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(ScheduleDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetScheduleById(int id)
    {
        var result = await _mediator.Send(new GetScheduleById.Query(id));
        return Ok(result);
    }
    
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateSchedule(int id, [FromBody] MutateScheduleDto updateDto)
    {
        await _mediator.Send(new UpdateSchedule.Command(id, updateDto));
        return NoContent();
    }
    
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteSchedule(int id)
    {
        await _mediator.Send(new DeleteSchedule.Command(id));
        return NoContent();
    }
}