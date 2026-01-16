using Poolup.Core.Entities.Authentication;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Poolup.Core.Entities.Waitlist;

[Table("waitlist_entries")]
public class WaitlistEntry
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; } = Guid.NewGuid();
 
    [Required]
    [Column("full_name")]
    [MaxLength(200)]
    public string FullName { get; set; } = default!;

    [Required]
    [Column("email")]
    [MaxLength(200)]
    public string Email { get; set; } = default!;

    [Required]
    [Column("origin")]
    [MaxLength(200)]
    public string Origin { get; set; } = default!;

    [Required]
    [Column("destination")]
    [MaxLength(200)]
    public string Destination { get; set; } = default!;

    [Required]
    [Column("time_window")]
    [MaxLength(200)]
    public string TimeWindow { get; set; } = default!;

    [Required]
    [Column("is_driver")]
    public bool IsDriver { get; set; }

    [Required]
    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}