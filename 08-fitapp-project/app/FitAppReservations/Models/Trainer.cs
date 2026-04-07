using System.ComponentModel.DataAnnotations;

namespace FitAppReservations.Models;

public class Trainer : BaseEntity
{
    [Required]
    [StringLength(120)]
    [Display(Name = "Full name")]
    public string FullName { get; set; } = string.Empty;

    [Required]
    [StringLength(120)]
    public string Specialty { get; set; } = string.Empty;

    [Range(typeof(decimal), "0", "99999999.99")]
    [Display(Name = "Hourly rate")]
    public decimal HourlyRate { get; set; }

    [Display(Name = "Active")]
    public bool IsActive { get; set; } = true;

    public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
}
