using MediatR;
using Microsoft.AspNetCore.Mvc;
using ScheduleApi.Application.DTOs.DayOfWeek;
using ScheduleApi.Application.Features.ApplicationDayOfWeek.Commands;
using ScheduleApi.Application.Features.ApplicationDayOfWeek.Queries;

namespace ScheduleApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DayOfWeekController : ControllerBase
{
    private readonly IMediator _mediator;

    public DayOfWeekController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateDayOfWeek([FromBody] CreateDayOfWeekDto createDto)
    {
        var newId = await _mediator.Send(new CreateDayOfWeek.Command(createDto));

        return CreatedAtAction(nameof(GetDayOfWeekById), new { id = newId }, new { id = newId });
    }

    [HttpGet]
    [ProducesResponseType(typeof(List<DayOfWeekDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetDaysOfWeek()
    {
        var result = await _mediator.Send(new GetDaysOfWeekList.Query());
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(DayOfWeekDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetDayOfWeekById(int id)
    {
        var result = await _mediator.Send(new GetDayOfWeekById.Query(id));
        return Ok(result);
    }
}