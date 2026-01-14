using MediatR;
using Poolup.Core.DTOs;
using Poolup.Core.Interfaces;

namespace Poolup.Application.Queries;
public record GetWaitlistEntriesQuery() : IRequest<List<WaitlistEntryDto>>;

public class GetWaitlistEntriesQueryHandler
    : IRequestHandler<GetWaitlistEntriesQuery, List<WaitlistEntryDto>>
{
    private readonly IWaitlistRepository _repo;
    public GetWaitlistEntriesQueryHandler(IWaitlistRepository repo) => _repo = repo;

    public async Task<List<WaitlistEntryDto>> Handle(GetWaitlistEntriesQuery request, CancellationToken ct)
    {
        var entries = await _repo.GetAllAsync();
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
