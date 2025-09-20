using MediatR;
using Microsoft.AspNetCore.Mvc;
using ScheduleApi.Application.DTOs.Semester;
using ScheduleApi.Application.Features.Semester.Commands;
using ScheduleApi.Application.Features.Semester.Queries;

namespace ScheduleApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SemesterController : ControllerBase
{
    private readonly IMediator _mediator;

    public SemesterController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateSemester([FromBody] CreateSemesterDto createDto)
    {
        var newId = await _mediator.Send(new CreateSemester.Command(createDto));

        return CreatedAtAction(nameof(GetSemesterById), new { id = newId }, new { id = newId });
    }

    [HttpGet]
    [ProducesResponseType(typeof(List<SemesterDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetSemesters()
    {
        var result = await _mediator.Send(new GetSemestersList.Query());
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(SemesterDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetSemesterById(int id)
    {
        var result = await _mediator.Send(new GetSemesterById.Query(id));
        return Ok(result);
    }
}