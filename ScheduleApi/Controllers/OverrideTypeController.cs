using MediatR;
using Microsoft.AspNetCore.Mvc;
using ScheduleApi.Application.DTOs.OverrideType;
using ScheduleApi.Application.Features.OverrideType.Commands;
using ScheduleApi.Application.Features.OverrideType.Queries;

namespace ScheduleApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OverrideTypeController : ControllerBase
{
    private readonly IMediator _mediator;

    public OverrideTypeController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateOverrideType([FromBody] CreateOverrideTypeDto createDto)
    {
        var newId = await _mediator.Send(new CreateOverrideType.Command(createDto));

        return CreatedAtAction(nameof(GetOverrideTypeById), new { id = newId }, new { id = newId });
    }

    [HttpGet]
    [ProducesResponseType(typeof(List<OverrideTypeDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetOverrideTypes()
    {
        var result = await _mediator.Send(new GetOverrideTypesList.Query());
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(OverrideTypeDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetOverrideTypeById(int id)
    {
        var result = await _mediator.Send(new GetOverrideTypeById.Query(id));
        return Ok(result);
    }
}