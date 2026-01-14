using Microsoft.EntityFrameworkCore;
using Poolup.Core.Entities.Authentication;  
using Poolup.Core.Entities.Waitlist;         

namespace Poolup.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users { get; set; } = default!;
    public DbSet<WaitlistEntry> WaitlistEntries { get; set; } = default!;
}
