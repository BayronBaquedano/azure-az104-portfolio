using FitAppReservations.Data;
using FitAppReservations.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace FitAppReservations.Pages;

public class IndexModel(ApplicationDbContext dbContext) : PageModel
{
    public int ActiveTrainersCount { get; private set; }

    public int ClientsCount { get; private set; }

    public int UpcomingReservationsCount { get; private set; }

    public IReadOnlyList<UpcomingReservationViewModel> UpcomingReservations { get; private set; } =
        Array.Empty<UpcomingReservationViewModel>();

    public async Task OnGetAsync()
    {
        var today = DateOnly.FromDateTime(DateTime.Today);

        ActiveTrainersCount = await dbContext.Trainers.CountAsync(x => x.IsActive);
        ClientsCount = await dbContext.Clients.CountAsync();
        UpcomingReservationsCount = await dbContext.Reservations.CountAsync(x =>
            x.ReservationDate >= today &&
            x.Status != ReservationStatus.Cancelled);

        UpcomingReservations = await dbContext.Reservations
            .AsNoTracking()
            .Where(x => x.ReservationDate >= today && x.Status != ReservationStatus.Cancelled)
            .OrderBy(x => x.ReservationDate)
            .ThenBy(x => x.StartTime)
            .Select(x => new UpcomingReservationViewModel(
                x.ReservationDate,
                x.StartTime,
                x.EndTime,
                x.Status,
                x.Trainer!.FullName,
                x.Client!.FullName))
            .Take(6)
            .ToListAsync();
    }

    public string GetStatusBadgeClass(ReservationStatus status) => status switch
    {
        ReservationStatus.Pending => "text-bg-warning",
        ReservationStatus.Confirmed => "text-bg-success",
        ReservationStatus.Cancelled => "text-bg-secondary",
        _ => "text-bg-light"
    };

    public sealed record UpcomingReservationViewModel(
        DateOnly ReservationDate,
        TimeOnly StartTime,
        TimeOnly EndTime,
        ReservationStatus Status,
        string TrainerName,
        string ClientName);
}
