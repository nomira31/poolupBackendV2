using MediatR;
using Microsoft.AspNetCore.Mvc;
using Poolup.Core.DTOs;
using Poolup.Application.Commands;
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

    /// </summary>
    [HttpPost("add")]
    public async Task<IActionResult> AddToWaitlist(AddWaitlistEntryCommand command)
    {
        try
        {
         
            var entry = await _mediator.Send(command);

            // If successful, return the object (200 OK)
            return Ok(entry);
        }
        catch (InvalidOperationException ex)
        {
            // This catches the "Email is already on the waitlist" error from your Handler
            // and returns it as a clean JSON message (400 Bad Request)
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            // This catches any other unexpected errors so the server doesn't send a raw crash page
            return StatusCode(500, new { message = "An internal error occurred.", details = ex.Message });
        }
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
        try
        {
            await _mediator.Send(new RemoveWaitlistEntryCommand(entryId));

            // Return a clear success message
            return Ok(new { message = $"Entry {entryId} successfully removed from the waitlist." });
        }
        catch (KeyNotFoundException ex) // Assuming your handler throws this if ID isn't found
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while removing the entry.", details = ex.Message });
        }
    }

}
