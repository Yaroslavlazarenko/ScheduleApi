using MediatR;
using Microsoft.AspNetCore.Mvc;
using ScheduleApi.Application.DTOs.Teacher;
using ScheduleApi.Application.DTOs.TeacherInfo;
using ScheduleApi.Application.DTOs.TeacherSubject;
using ScheduleApi.Application.Features.Teacher.Commands;
using ScheduleApi.Application.Features.Teacher.Queries;
using ScheduleApi.Application.Features.TeacherInfo.Commands;
using ScheduleApi.Application.Features.TeacherSubject.Commands;
using ScheduleApi.Application.Features.TeacherSubject.Queries;

namespace ScheduleApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TeacherController : ControllerBase
{
    private readonly IMediator _mediator;

    public TeacherController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateTeacher([FromBody] CreateTeacherDto createDto)
    {
        var newId = await _mediator.Send(new CreateTeacher.Command(createDto));

        return CreatedAtAction(nameof(GetTeacherById), new { id = newId }, new { id = newId });
    }

    [HttpGet]
    [ProducesResponseType(typeof(List<TeacherDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetTeachers()
    {
        var result = await _mediator.Send(new GetTeachersList.Query());
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(TeacherDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetTeacherById(int id)
    {
        var result = await _mediator.Send(new GetTeacherById.Query(id));
        return Ok(result);
    }
    
    [HttpPost("{teacherId:int}/info")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AddInfoToTeacher(int teacherId, [FromBody] CreateTeacherInfoDto createDto)
    {
        await _mediator.Send(new AddInfoToTeacher.Command(teacherId, createDto));
        return NoContent();
    }

    [HttpPut("{teacherId:int}/info/{infoTypeId:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateInfoForTeacher(int teacherId, int infoTypeId, [FromBody] UpdateTeacherInfoDto updateDto)
    {
        await _mediator.Send(new UpdateInfoForTeacher.Command(teacherId, infoTypeId, updateDto));
        return NoContent();
    }

    [HttpDelete("{teacherId:int}/info/{infoTypeId:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteInfoFromTeacher(int teacherId, int infoTypeId)
    {
        await _mediator.Send(new DeleteInfoFromTeacher.Command(teacherId, infoTypeId));
        return NoContent();
    }
    
    [HttpPost("{teacherId:int}/subjects")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AssignSubjectToTeacher(int teacherId, [FromBody] AssignSubjectToTeacherDto assignDto)
    {
        await _mediator.Send(new AssignSubjectToTeacher.Command(teacherId, assignDto));
        return NoContent();
    }

    [HttpGet("{teacherId:int}/subjects")]
    [ProducesResponseType(typeof(List<TeacherSubjectDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetSubjectsForTeacher(int teacherId)
    {
        var result = await _mediator.Send(new GetSubjectsForTeacher.Query(teacherId));
        return Ok(result);
    }
    
    [HttpPut("{teacherId:int}/subjects/{subjectId:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateSubjectForTeacher(int teacherId, int subjectId, [FromBody] UpdateTeacherSubjectDto updateDto)
    {
        await _mediator.Send(new UpdateSubjectForTeacher.Command(teacherId, subjectId, updateDto));
        return NoContent();
    }

    [HttpDelete("{teacherId:int}/subjects/{subjectId:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UnassignSubjectFromTeacher(int teacherId, int subjectId)
    {
        await _mediator.Send(new UnassignSubjectFromTeacher.Command(teacherId, subjectId));
        return NoContent();
    }
}