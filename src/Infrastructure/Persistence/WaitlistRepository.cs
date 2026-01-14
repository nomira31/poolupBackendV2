using Microsoft.EntityFrameworkCore;
using Poolup.Core.Entities.Waitlist;
using Poolup.Core.Interfaces;

namespace Poolup.Infrastructure.Persistence;
public class WaitlistRepository : IWaitlistRepository
{
    private readonly ApplicationDbContext _db;

    public WaitlistRepository(ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task AddAsync(WaitlistEntry entry, CancellationToken ct)
    {
        _db.WaitlistEntries.Add(entry);
        await _db.SaveChangesAsync(ct);
    }

    public async Task RemoveAsync(Guid entryId, CancellationToken ct)
    {
        var entry = await _db.WaitlistEntries.FindAsync(entryId);
        if (entry != null)
        {
            _db.WaitlistEntries.Remove(entry);
            await _db.SaveChangesAsync(ct);
        }
    }

    public async Task<List<WaitlistEntry>> GetAllAsync()
        => await _db.WaitlistEntries.Include(e => e.User).ToListAsync();

    public async Task<List<WaitlistEntry>> GetByUserIdAsync(Guid userId)
        => await _db.WaitlistEntries.Where(e => e.UserId == userId).ToListAsync();

    public async Task<WaitlistEntry?> GetByIdAsync(Guid id)
        => await _db.WaitlistEntries.Include(e => e.User).FirstOrDefaultAsync(e => e.Id == id);
}
