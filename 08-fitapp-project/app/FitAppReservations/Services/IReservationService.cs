using FitAppReservations.Models;

namespace FitAppReservations.Services;

public interface IReservationService
{
    Task<ReservationValidationResult> ValidateAsync(
        Reservation reservation,
        CancellationToken cancellationToken = default);
}
