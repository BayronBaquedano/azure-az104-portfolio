using FitAppReservations.Data;
using FitAppReservations.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace FitAppReservations.Pages.Reservations;

public class IndexModel(
    ApplicationDbContext dbContext,
    ILogger<IndexModel> logger) : PageModel
{
    [BindProperty(SupportsGet = true)]
    public int? TrainerId { get; set; }

    [BindProperty(SupportsGet = true)]
    public DateOnly? ReservationDate { get; set; }

    [BindProperty(SupportsGet = true)]
    public ReservationStatus? Status { get; set; }

    public IReadOnlyList<SelectListItem> TrainerOptions { get; private set; } = Array.Empty<SelectListItem>();

    public IReadOnlyList<ReservationListItem> Reservations { get; private set; } = Array.Empty<ReservationListItem>();

    public async Task OnGetAsync()
    {
        await LoadTrainerOptionsAsync();

        var query = dbContext.Reservations
            .AsNoTracking();

        if (TrainerId.HasValue)
        {
            query = query.Where(x => x.TrainerId == TrainerId.Value);
        }

        if (ReservationDate.HasValue)
        {
            query = query.Where(x => x.ReservationDate == ReservationDate.Value);
        }

        if (Status.HasValue)
        {
            query = query.Where(x => x.Status == Status.Value);
        }

        Reservations = await query
            .OrderBy(x => x.ReservationDate)
            .ThenBy(x => x.StartTime)
            .Select(x => new ReservationListItem(
                x.Id,
                x.ReservationDate,
                x.StartTime,
                x.EndTime,
                x.Status,
                x.Notes,
                x.Trainer!.FullName,
                x.Client!.FullName))
            .ToListAsync();
    }

    public async Task<IActionResult> OnPostDeleteAsync(int id)
    {
        var reservation = await dbContext.Reservations.FirstOrDefaultAsync(x => x.Id == id);
        if (reservation is null)
        {
            return NotFound();
        }

        dbContext.Reservations.Remove(reservation);
        await dbContext.SaveChangesAsync();

        logger.LogInformation("Reservation {ReservationId} deleted.", reservation.Id);
        TempData["StatusMessage.Success"] = "Reservation deleted.";
        return RedirectToPage();
    }

    public string GetStatusBadgeClass(ReservationStatus status) => status switch
    {
        ReservationStatus.Pending => "text-bg-warning",
        ReservationStatus.Confirmed => "text-bg-success",
        ReservationStatus.Cancelled => "text-bg-secondary",
        _ => "text-bg-light"
    };

    private async Task LoadTrainerOptionsAsync()
    {
        TrainerOptions = await dbContext.Trainers
            .AsNoTracking()
            .OrderBy(x => x.FullName)
            .Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.FullName
            })
            .ToListAsync();
    }

    public sealed record ReservationListItem(
        int Id,
        DateOnly ReservationDate,
        TimeOnly StartTime,
        TimeOnly EndTime,
        ReservationStatus Status,
        string? Notes,
        string TrainerName,
        string ClientName);
}
