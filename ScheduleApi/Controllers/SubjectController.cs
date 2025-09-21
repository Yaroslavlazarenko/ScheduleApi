using MediatR;
using Microsoft.AspNetCore.Mvc;
using ScheduleApi.Application.DTOs.Subject;
using ScheduleApi.Application.Features.Subject.Commands;
using ScheduleApi.Application.Features.Subject.Queries;

namespace ScheduleApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SubjectController : ControllerBase
{
    private readonly IMediator _mediator;

    public SubjectController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CreateSubject([FromBody] CreateSubjectDto createDto)
    {
        var newId = await _mediator.Send(new CreateSubject.Command(createDto));

        return CreatedAtAction(nameof(GetSubjectById), new { id = newId }, new { id = newId });
    }

    [HttpGet]
    [ProducesResponseType(typeof(List<SubjectDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetSubjects()
    {
        var result = await _mediator.Send(new GetSubjectsList.Query());
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(SubjectDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetSubjectById(int id)
    {
        var result = await _mediator.Send(new GetSubjectById.Query(id));
        return Ok(result);
    }
    
    [HttpGet("{id:int}/info")]
    [ProducesResponseType(typeof(SubjectDetailsDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetSubjectInfo(int id)
    {
        var result = await _mediator.Send(new GetSubjectDetails.Query(id));
        return Ok(result);
    }
}