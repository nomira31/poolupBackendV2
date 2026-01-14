using Poolup.Core.Entities.Authentication;

namespace Poolup.Core.Entities.Waitlist;

public class WaitlistEntry
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid UserId { get; set; }            
    public User? User { get; set; }            

    public string FullName { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string Origin { get; set; } = default!;
    public string Destination { get; set; } = default!;
    public string TimeWindow { get; set; } = default!;
    public bool IsDriver { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
