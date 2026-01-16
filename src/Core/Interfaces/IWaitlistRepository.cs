namespace Poolup.Core.Interfaces;


using Poolup.Core.Entities.Waitlist;

public interface IWaitlistRepository
{
    Task AddAsync(WaitlistEntry entry, CancellationToken ct);
    Task RemoveAsync(Guid entryId, CancellationToken ct);
    Task<List<WaitlistEntry>> GetAllAsync();
    Task<bool> EmailExistsAsync(string email, CancellationToken ct);
}
