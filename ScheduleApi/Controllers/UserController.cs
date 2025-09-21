using MediatR;
using Microsoft.AspNetCore.Mvc;
using ScheduleApi.Application.DTOs.Schedule;
using ScheduleApi.Application.DTOs.User;
using ScheduleApi.Application.Features.Schedule.Queries;
using ScheduleApi.Application.Features.User.Commands;
using ScheduleApi.Application.Features.User.Queries;

namespace ScheduleApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IMediator _mediator;

    public UserController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserDto createUserDto)
    {
        var newUserId = await _mediator.Send(new CreateUser.Command(createUserDto));

        return CreatedAtAction(nameof(GetUserById), new { id = newUserId }, new { id = newUserId });
    }
    
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetUserById(int id)
    {
        var userDto = await _mediator.Send(new GetUserById.Query(id));
        
        return Ok(userDto);
    }
    
    [HttpGet("telegram/{telegramId:long}")]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetUserByTelegramId(long telegramId)
    {
        var userDto = await _mediator.Send(new GetUserByTelegramId.Query(telegramId));
        
        return Ok(userDto);
    }
    
    [HttpGet("{userId:int}/schedule")]
    [ProducesResponseType(typeof(List<DailyScheduleDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetScheduleForDay(int userId, [FromQuery] DateTime? date)
    {
        var result = await _mediator.Send(new GetDailyScheduleForUser.Query(userId, date));
    
        return Ok(result);
    }
    
    [HttpPut("{userId:int}/group")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ChangeUserGroup(int userId, [FromBody] ChangeUserGroupDto changeGroupDto)
    {
        await _mediator.Send(new ChangeUserGroup.Command(userId, changeGroupDto));
        return NoContent();
    }

    [HttpPut("{userId:int}/region")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ChangeUserRegion(int userId, [FromBody] ChangeUserRegionDto changeRegionDto)
    {
        await _mediator.Send(new ChangeUserRegion.Command(userId, changeRegionDto));
        return NoContent();
    }
}