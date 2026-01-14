using MediatR;
using Poolup.Core.DTOs;
using Poolup.Core.Interfaces;

namespace Poolup.Application.Queries;

public record GetWaitlistEntriesByUserQuery(Guid UserId) : IRequest<List<WaitlistEntryDto>>;

public class GetWaitlistEntriesByUserQueryHandler
    : IRequestHandler<GetWaitlistEntriesByUserQuery, List<WaitlistEntryDto>>
{
    private readonly IWaitlistRepository _repo;

    public GetWaitlistEntriesByUserQueryHandler(IWaitlistRepository repo)
    {
        _repo = repo;
    }

    public async Task<List<WaitlistEntryDto>> Handle(GetWaitlistEntriesByUserQuery request, CancellationToken ct)
    {
        var entries = await _repo.GetByUserIdAsync(request.UserId);

        return entries.Select(e => new WaitlistEntryDto
        {
            Id = e.Id,
            UserFullName = e.FullName ?? "Unknown.",
            Origin = e.Origin,
            Destination = e.Destination,
            TimeWindow = e.TimeWindow,
            IsDriver = e.IsDriver
        }).ToList();
    }
}
