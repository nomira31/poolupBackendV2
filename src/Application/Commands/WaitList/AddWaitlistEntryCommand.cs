using MediatR;
using Poolup.Core.Entities.Waitlist;
using Poolup.Core.Interfaces;

namespace Poolup.Application.Commands;

public record AddWaitlistEntryCommand(
    Guid UserId,
    string FullName,
    string Email,
    string Origin,
    string Destination,
    string TimeWindow,
    bool IsDriver
) : IRequest<Guid>;


public class AddWaitlistEntryCommandHandler
    : IRequestHandler<AddWaitlistEntryCommand, Guid>
{
    private readonly IWaitlistRepository _repo;

    public AddWaitlistEntryCommandHandler(IWaitlistRepository repo)
    {
        _repo = repo;
    }

    public async Task<Guid> Handle(AddWaitlistEntryCommand request, CancellationToken ct)
    {
        var entry = new WaitlistEntry
        {
            UserId = request.UserId,
            FullName = request.FullName,
            Email = request.Email,
            Origin = request.Origin,
            Destination = request.Destination,
            TimeWindow = request.TimeWindow,
            IsDriver = request.IsDriver,
            CreatedAt = DateTime.UtcNow  
        };

        await _repo.AddAsync(entry, ct);

        return entry.Id;
    }
}