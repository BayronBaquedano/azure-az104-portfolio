using System.ComponentModel.DataAnnotations;

namespace FitAppReservations.Models;

public class ReservationFormInput
{
    [Range(1, int.MaxValue, ErrorMessage = "Select a trainer.")]
    [Display(Name = "Trainer")]
    public int TrainerId { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Select a client.")]
    [Display(Name = "Client")]
    public int ClientId { get; set; }

    [Display(Name = "Reservation date")]
    public DateOnly ReservationDate { get; set; } = DateOnly.FromDateTime(DateTime.Today);

    [Display(Name = "Start time")]
    public TimeOnly StartTime { get; set; } = new(9, 0);

    [Display(Name = "End time")]
    public TimeOnly EndTime { get; set; } = new(10, 0);

    public ReservationStatus Status { get; set; } = ReservationStatus.Pending;

    [StringLength(1000)]
    public string? Notes { get; set; }
}
