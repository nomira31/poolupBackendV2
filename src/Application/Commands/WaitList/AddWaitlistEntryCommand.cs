using MediatR;
using Poolup.Core.Entities.Waitlist;
using Poolup.Core.Interfaces;

namespace Poolup.Application.Commands;

// 1. Change return type from Guid to WaitlistEntry
public record AddWaitlistEntryCommand(
    string FullName,
    string Email,
    string Origin,
    string Destination,
    string TimeWindow,
    bool IsDriver
) : IRequest<WaitlistEntry>; 

public class AddWaitlistEntryCommandHandler
    : IRequestHandler<AddWaitlistEntryCommand, WaitlistEntry>  
{
    private readonly IWaitlistRepository _repo;

    public AddWaitlistEntryCommandHandler(IWaitlistRepository repo)
    {
        _repo = repo;
    }

    public async Task<WaitlistEntry> Handle(AddWaitlistEntryCommand request, CancellationToken ct)
    {
        var exists = await _repo.EmailExistsAsync(request.Email, ct);

        if (exists)
            throw new InvalidOperationException(
                $"Email '{request.Email}' is already on the waitlist.");

        var entry = new WaitlistEntry
        {
            Id = Guid.NewGuid(),
            FullName = request.FullName,
            Email = request.Email,
            Origin = request.Origin,
            Destination = request.Destination,
            TimeWindow = request.TimeWindow,
            IsDriver = request.IsDriver,
            CreatedAt = DateTime.UtcNow
         
        };

        await _repo.AddAsync(entry, ct);

        return entry;  
    }
}