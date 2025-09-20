using MediatR;
using Microsoft.AspNetCore.Mvc;
using ScheduleApi.Application.DTOs.SocialMediaType;
using ScheduleApi.Application.Features.SocialMediaType.Commands;
using ScheduleApi.Application.Features.SocialMediaType.Queries;

namespace ScheduleApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SocialMediaTypeController : ControllerBase
{
    private readonly IMediator _mediator;

    public SocialMediaTypeController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateSocialMediaType([FromBody] CreateSocialMediaTypeDto createDto)
    {
        var newId = await _mediator.Send(new CreateSocialMediaType.Command(createDto));

        return CreatedAtAction(nameof(GetSocialMediaTypeById), new { id = newId }, new { id = newId });
    }

    [HttpGet]
    [ProducesResponseType(typeof(List<SocialMediaTypeDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetSocialMediaTypes()
    {
        var result = await _mediator.Send(new GetSocialMediaTypesList.Query());
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(SocialMediaTypeDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetSocialMediaTypeById(int id)
    {
        var result = await _mediator.Send(new GetSocialMediaTypeById.Query(id));
        return Ok(result);
    }
}