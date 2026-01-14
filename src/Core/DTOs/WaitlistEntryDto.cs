namespace Poolup.Core.DTOs;

public class WaitlistEntryDto
{
    public Guid Id { get; set; }
    public string UserFullName { get; set; } = default!;
    public string Origin { get; set; } = default!;
    public string Destination { get; set; } = default!;
    public string TimeWindow { get; set; } = default!;
    public bool IsDriver { get; set; }
}
