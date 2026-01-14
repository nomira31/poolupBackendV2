using MediatR;
using Microsoft.AspNetCore.Mvc;
using Poolup.Core.DTOs;
using Poolup.Application.Commands ;
using Poolup.Application.Queries;

namespace Poolup.Api.Controllers;

[ApiController]
[Route("api/waitlist")]
public class WaitlistController : ControllerBase
{
    private readonly IMediator _mediator;

    public WaitlistController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Add a user or driver to the waitlist
    /// </summary>
    [HttpPost("add")]
    public async Task<IActionResult> AddToWaitlist(AddWaitlistEntryCommand command)
    {
        var entryId = await _mediator.Send(command);
        return Ok(new { EntryId = entryId });
    }

    /// <summary>
    /// Get all waitlist entries (admin)
    /// </summary>
    [HttpGet("all")]
    public async Task<IActionResult> GetAllWaitlistEntries()
    {
        var entries = await _mediator.Send(new GetWaitlistEntriesQuery());
        return Ok(entries);
    }

    /// <summary>
    /// Remove a waitlist entry (admin)
    /// </summary>
    [HttpDelete("{entryId:guid}")]
    public async Task<IActionResult> RemoveWaitlistEntry(Guid entryId)
    {
        await _mediator.Send(new RemoveWaitlistEntryCommand(entryId));
        return NoContent();
    }

    /// <summary>
    /// Get waitlist entries for a specific user
    /// </summary>
    [HttpGet("user/{userId:guid}")]
    public async Task<IActionResult> GetByUserId(Guid userId)
    {
        var entries = await _mediator.Send(new GetWaitlistEntriesByUserQuery(userId));
        return Ok(entries);
    }
}
