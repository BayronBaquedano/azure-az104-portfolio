using FitAppReservations.Data;
using FitAppReservations.Models;
using Microsoft.EntityFrameworkCore;

namespace FitAppReservations.Services;

public class ReservationService(
    ApplicationDbContext dbContext,
    ILogger<ReservationService> logger) : IReservationService
{
    public async Task<ReservationValidationResult> ValidateAsync(
        Reservation reservation,
        CancellationToken cancellationToken = default)
    {
        var errors = new List<string>();

        if (reservation.EndTime <= reservation.StartTime)
        {
            errors.Add("Reservation end time must be after the start time.");
        }

        var trainer = await dbContext.Trainers
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == reservation.TrainerId, cancellationToken);

        if (trainer is null)
        {
            errors.Add("The selected trainer no longer exists.");
        }
        else if (!trainer.IsActive && reservation.Status != ReservationStatus.Cancelled)
        {
            errors.Add("Only active trainers can receive new reservations.");
        }

        var clientExists = await dbContext.Clients
            .AsNoTracking()
            .AnyAsync(x => x.Id == reservation.ClientId, cancellationToken);

        if (!clientExists)
        {
            errors.Add("The selected client no longer exists.");
        }

        if (!errors.Any() && reservation.Status != ReservationStatus.Cancelled)
        {
            var overlaps = await dbContext.Reservations
                .AsNoTracking()
                .Where(x =>
                    x.Id != reservation.Id &&
                    x.TrainerId == reservation.TrainerId &&
                    x.ReservationDate == reservation.ReservationDate &&
                    x.Status != ReservationStatus.Cancelled &&
                    x.StartTime < reservation.EndTime &&
                    reservation.StartTime < x.EndTime)
                .AnyAsync(cancellationToken);

            if (overlaps)
            {
                errors.Add("The trainer already has another reservation that overlaps with this time slot.");
            }
        }

        if (errors.Any())
        {
            logger.LogWarning(
                "Reservation validation failed for trainer {TrainerId}, client {ClientId}, date {ReservationDate}, start {StartTime}, end {EndTime}. Errors: {Errors}",
                reservation.TrainerId,
                reservation.ClientId,
                reservation.ReservationDate,
                reservation.StartTime,
                reservation.EndTime,
                errors);

            return ReservationValidationResult.Failure(errors);
        }

        return ReservationValidationResult.Success();
    }
}
