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


    public async Task<bool> EmailExistsAsync(string email, CancellationToken ct)
    {
        return await _db.WaitlistEntries
            .AnyAsync(w => w.Email == email, ct);
    }

    public async Task<List<WaitlistEntry>> GetAllAsync()
        => await _db.WaitlistEntries.ToListAsync();

   
  
}
