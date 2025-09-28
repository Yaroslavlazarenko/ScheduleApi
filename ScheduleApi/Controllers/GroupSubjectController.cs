using MediatR;
using Microsoft.AspNetCore.Mvc;
using ScheduleApi.Application.Features.GroupSubject.Commands;

namespace ScheduleApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GroupSubjectController : ControllerBase
{
    private readonly IMediator _mediator;

    public GroupSubjectController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpDelete("{Id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UnassignSubjectFromGroup(int groupSubjectId)
    {
        await _mediator.Send(new UnassignSubjectFromGroup.Command(groupSubjectId));
        return NoContent();
    }
}