using MediatR;
using Microsoft.AspNetCore.Mvc;
using ScheduleApi.Application.DTOs;
using ScheduleApi.Application.DTOs.Group;
using ScheduleApi.Application.DTOs.GroupSubject;
using ScheduleApi.Application.Features.Group.Commands;
using ScheduleApi.Application.Features.Group.Queries;
using ScheduleApi.Application.Features.GroupSubject.Commands;
using ScheduleApi.Application.Features.GroupSubject.Queries;

namespace ScheduleApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GroupController : ControllerBase
{
    private readonly IMediator _mediator;

    public GroupController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateGroup([FromBody] CreateGroupDto createGroupDto)
    {
        var newGroupId = await _mediator.Send(new CreateGroup.Command(createGroupDto));

        return CreatedAtAction(nameof(GetGroupById), new { id = newGroupId }, new { id = newGroupId });
    }

    [HttpGet]
    [ProducesResponseType(typeof(List<GroupDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetGroups()
    {
        var result = await _mediator.Send(new GetGroupsList.Query());

        return Ok(result);
    }
    
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(GroupDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetGroupById(int id)
    {
        var group = await _mediator.Send(new GetGroupById.Query(id));
        
        return Ok(group);
    }
    
    [HttpPost("{groupId:int}/subjects")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AssignSubjectToGroup(int groupId, [FromBody] AssignSubjectToGroupDto assignDto)
    {
        await _mediator.Send(new AssignSubjectToGroup.Command(groupId, assignDto));
        return NoContent();
    }

    [HttpGet("{groupId:int}/subjects")]
    [ProducesResponseType(typeof(List<GroupSubjectDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetSubjectsForGroup(int groupId)
    {
        var result = await _mediator.Send(new GetSubjectsForGroup.Query(groupId));
        return Ok(result);
    }
}