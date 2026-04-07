using System.ComponentModel.DataAnnotations;

namespace FitAppReservations.Models;

public class Reservation : BaseEntity
{
    [Display(Name = "Trainer")]
    public int TrainerId { get; set; }

    [Display(Name = "Client")]
    public int ClientId { get; set; }

    [Display(Name = "Reservation date")]
    public DateOnly ReservationDate { get; set; }

    [Display(Name = "Start time")]
    public TimeOnly StartTime { get; set; }

    [Display(Name = "End time")]
    public TimeOnly EndTime { get; set; }

    public ReservationStatus Status { get; set; } = ReservationStatus.Pending;

    [StringLength(1000)]
    public string? Notes { get; set; }

    public Trainer? Trainer { get; set; }

    public Client? Client { get; set; }
}
