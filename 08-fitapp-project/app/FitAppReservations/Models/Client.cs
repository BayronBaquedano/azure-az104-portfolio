using System.ComponentModel.DataAnnotations;

namespace FitAppReservations.Models;

public class Client : BaseEntity
{
    [Required]
    [StringLength(120)]
    [Display(Name = "Full name")]
    public string FullName { get; set; } = string.Empty;

    [EmailAddress]
    [StringLength(200)]
    public string? Email { get; set; }

    [StringLength(50)]
    public string? Phone { get; set; }

    [StringLength(1000)]
    public string? Notes { get; set; }

    public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
}
