
namespace Poolup.Core.DTOs;

public class UserProfileDto
{
    public Guid Id { get; set; }
    public string FullName { get; set; } = default!;
    public string Email { get; set; } = default!;
}
