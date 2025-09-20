using MediatR;
using Microsoft.AspNetCore.Mvc;
using ScheduleApi.Application.DTOs.SubjectType;
using ScheduleApi.Application.Features.SubjectType.Commands;
using ScheduleApi.Application.Features.SubjectType.Queries;

namespace ScheduleApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SubjectTypeController : ControllerBase
{
    private readonly IMediator _mediator;

    public SubjectTypeController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateSubjectType([FromBody] CreateSubjectTypeDto createDto)
    {
        var newId = await _mediator.Send(new CreateSubjectType.Command(createDto));

        return CreatedAtAction(nameof(GetSubjectTypeById), new { id = newId }, new { id = newId });
    }

    [HttpGet]
    [ProducesResponseType(typeof(List<SubjectTypeDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetSubjectTypes()
    {
        var result = await _mediator.Send(new GetSubjectTypesList.Query());
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(SubjectTypeDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetSubjectTypeById(int id)
    {
        var result = await _mediator.Send(new GetSubjectTypeById.Query(id));
        return Ok(result);
    }
}