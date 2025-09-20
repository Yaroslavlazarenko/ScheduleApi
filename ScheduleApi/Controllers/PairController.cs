using MediatR;
using Microsoft.AspNetCore.Mvc;
using ScheduleApi.Application.DTOs.Pair;
using ScheduleApi.Application.Features.Pair.Commands;
using ScheduleApi.Application.Features.Pair.Queries;

namespace ScheduleApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PairController : ControllerBase
{
    private readonly IMediator _mediator;

    public PairController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreatePair([FromBody] CreatePairDto createDto)
    {
        var newId = await _mediator.Send(new CreatePair.Command(createDto));

        return CreatedAtAction(nameof(GetPairById), new { id = newId }, new { id = newId });
    }

    [HttpGet]
    [ProducesResponseType(typeof(List<PairDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetPairs()
    {
        var result = await _mediator.Send(new GetPairsList.Query());
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(PairDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetPairById(int id)
    {
        var result = await _mediator.Send(new GetPairById.Query(id));
        return Ok(result);
    }
}