using FitAppReservations.Data;
using FitAppReservations.Models;
using FitAppReservations.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace FitAppReservations.Pages.Reservations;

public class EditModel(
    ApplicationDbContext dbContext,
    IReservationService reservationService,
    ILogger<EditModel> logger) : PageModel
{
    [BindProperty]
    public ReservationFormInput Input { get; set; } = new();

    public IReadOnlyList<SelectListItem> TrainerOptions { get; private set; } = Array.Empty<SelectListItem>();

    public IReadOnlyList<SelectListItem> ClientOptions { get; private set; } = Array.Empty<SelectListItem>();

    public async Task<IActionResult> OnGetAsync(int id)
    {
        var reservation = await dbContext.Reservations
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id);

        if (reservation is null)
        {
            return NotFound();
        }

        Input = new ReservationFormInput
        {
            TrainerId = reservation.TrainerId,
            ClientId = reservation.ClientId,
            ReservationDate = reservation.ReservationDate,
            StartTime = reservation.StartTime,
            EndTime = reservation.EndTime,
            Status = reservation.Status,
            Notes = reservation.Notes
        };

        await LoadLookupsAsync(reservation.TrainerId);
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int id)
    {
        await LoadLookupsAsync(Input.TrainerId);

        if (!ModelState.IsValid)
        {
            return Page();
        }

        var reservation = await dbContext.Reservations.FirstOrDefaultAsync(x => x.Id == id);
        if (reservation is null)
        {
            return NotFound();
        }

        reservation.TrainerId = Input.TrainerId;
        reservation.ClientId = Input.ClientId;
        reservation.ReservationDate = Input.ReservationDate;
        reservation.StartTime = Input.StartTime;
        reservation.EndTime = Input.EndTime;
        reservation.Status = Input.Status;
        reservation.Notes = string.IsNullOrWhiteSpace(Input.Notes) ? null : Input.Notes.Trim();

        var validationResult = await reservationService.ValidateAsync(reservation);
        if (!validationResult.IsValid)
        {
            foreach (var error in validationResult.Errors)
            {
                ModelState.AddModelError(string.Empty, error);
            }

            return Page();
        }

        await dbContext.SaveChangesAsync();

        logger.LogInformation("Reservation {ReservationId} updated.", reservation.Id);
        TempData["StatusMessage.Success"] = "Reservation updated.";
        return RedirectToPage("Index");
    }

    private async Task LoadLookupsAsync(int selectedTrainerId)
    {
        TrainerOptions = await dbContext.Trainers
            .AsNoTracking()
            .Where(x => x.IsActive || x.Id == selectedTrainerId)
            .OrderBy(x => x.FullName)
            .Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.IsActive ? x.FullName : $"{x.FullName} (inactive)"
            })
            .ToListAsync();

        ClientOptions = await dbContext.Clients
            .AsNoTracking()
            .OrderBy(x => x.FullName)
            .Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.FullName
            })
            .ToListAsync();
    }
}
