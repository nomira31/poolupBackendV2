using MediatR;
using Poolup.Core.Interfaces;

namespace Poolup.Application.Commands;
public record RemoveWaitlistEntryCommand(Guid EntryId) : IRequest<Unit>;  

public class RemoveWaitlistEntryCommandHandler
    : IRequestHandler<RemoveWaitlistEntryCommand, Unit>   
{
    private readonly IWaitlistRepository _repo;
    public RemoveWaitlistEntryCommandHandler(IWaitlistRepository repo) => _repo = repo;

    public async Task<Unit> Handle(RemoveWaitlistEntryCommand request, CancellationToken ct)
    {
        await _repo.RemoveAsync(request.EntryId, ct);
        return Unit.Value;
    }
}
