using FitAppReservations.Data;
using FitAppReservations.Models;
using FitAppReservations.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace FitAppReservations.Pages.Reservations;

public class CreateModel(
    ApplicationDbContext dbContext,
    IReservationService reservationService,
    ILogger<CreateModel> logger) : PageModel
{
    [BindProperty]
    public ReservationFormInput Input { get; set; } = new();

    public IReadOnlyList<SelectListItem> TrainerOptions { get; private set; } = Array.Empty<SelectListItem>();

    public IReadOnlyList<SelectListItem> ClientOptions { get; private set; } = Array.Empty<SelectListItem>();

    public bool CanCreateReservation => TrainerOptions.Count > 0 && ClientOptions.Count > 0;

    public async Task OnGetAsync()
    {
        await LoadLookupsAsync();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        await LoadLookupsAsync();

        if (!ModelState.IsValid)
        {
            return Page();
        }

        var reservation = new Reservation
        {
            TrainerId = Input.TrainerId,
            ClientId = Input.ClientId,
            ReservationDate = Input.ReservationDate,
            StartTime = Input.StartTime,
            EndTime = Input.EndTime,
            Status = Input.Status,
            Notes = string.IsNullOrWhiteSpace(Input.Notes) ? null : Input.Notes.Trim()
        };

        var validationResult = await reservationService.ValidateAsync(reservation);
        if (!validationResult.IsValid)
        {
            foreach (var error in validationResult.Errors)
            {
                ModelState.AddModelError(string.Empty, error);
            }

            return Page();
        }

        dbContext.Reservations.Add(reservation);
        await dbContext.SaveChangesAsync();

        logger.LogInformation(
            "Reservation {ReservationId} created for trainer {TrainerId} and client {ClientId}.",
            reservation.Id,
            reservation.TrainerId,
            reservation.ClientId);

        TempData["StatusMessage.Success"] = "Reservation created.";
        return RedirectToPage("Index");
    }

    private async Task LoadLookupsAsync()
    {
        TrainerOptions = await dbContext.Trainers
            .AsNoTracking()
            .Where(x => x.IsActive)
            .OrderBy(x => x.FullName)
            .Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.FullName
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
