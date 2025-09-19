using MediatR;
using Microsoft.AspNetCore.Mvc;
using ScheduleApi.Application.DTOs.Region;
using ScheduleApi.Application.Features.Region.Commands;
using ScheduleApi.Application.Features.Region.Queries;

namespace ScheduleApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RegionController : ControllerBase
{
    private readonly IMediator _mediator;

    public RegionController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateRegion([FromBody] CreateRegionDto createDto)
    {
        var newId = await _mediator.Send(new CreateRegion.Command(createDto));

        return CreatedAtAction(nameof(GetRegionById), new { id = newId }, new { id = newId });
    }

    [HttpGet]
    [ProducesResponseType(typeof(List<RegionDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetRegions()
    {
        var result = await _mediator.Send(new GetRegionsList.Query());
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(RegionDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetRegionById(int id)
    {
        var result = await _mediator.Send(new GetRegionById.Query(id));
        return Ok(result);
    }
}