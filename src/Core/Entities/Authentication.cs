namespace Poolup.Core.Entities.Authentication;

public class User
{
    public Guid Id { get; set; } = Guid.NewGuid(); // primary key
    public string FullName { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string PasswordHash { get; set; } = default!;

    // Optional: add more fields as needed
    public string? PhoneNumber { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
