using MediatR;
using Microsoft.AspNetCore.Mvc;
using ScheduleApi.Application.DTOs.InfoType;
using ScheduleApi.Application.Features.InfoType.Commands;
using ScheduleApi.Application.Features.InfoType.Queries;

namespace ScheduleApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class InfoTypesController : ControllerBase
{
    private readonly IMediator _mediator;

    public InfoTypesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateInfoType([FromBody] CreateInfoTypeDto createDto)
    {
        var newId = await _mediator.Send(new CreateInfoType.Command(createDto));

        return CreatedAtAction(nameof(GetInfoTypeById), new { id = newId }, new { id = newId });
    }

    [HttpGet]
    [ProducesResponseType(typeof(List<InfoTypeDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetInfoTypes()
    {
        var result = await _mediator.Send(new GetInfoTypesList.Query());
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(InfoTypeDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetInfoTypeById(int id)
    {
        var result = await _mediator.Send(new GetInfoTypeById.Query(id));
        return Ok(result);
    }
}