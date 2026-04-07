using FitAppReservations.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace FitAppReservations.Pages.Trainers;

public class IndexModel(
    ApplicationDbContext dbContext,
    ILogger<IndexModel> logger) : PageModel
{
    [BindProperty(SupportsGet = true)]
    public string? Search { get; set; }

    public IReadOnlyList<TrainerListItem> Trainers { get; private set; } = Array.Empty<TrainerListItem>();

    public async Task OnGetAsync()
    {
        var query = dbContext.Trainers
            .AsNoTracking();

        if (!string.IsNullOrWhiteSpace(Search))
        {
            var searchTerm = Search.Trim();
            query = query.Where(x =>
                x.FullName.Contains(searchTerm) ||
                x.Specialty.Contains(searchTerm));
        }

        Trainers = await query
            .OrderBy(x => x.FullName)
            .Select(x => new TrainerListItem(
                x.Id,
                x.FullName,
                x.Specialty,
                x.HourlyRate,
                x.IsActive,
                dbContext.Reservations.Count(r => r.TrainerId == x.Id)))
            .ToListAsync();
    }

    public async Task<IActionResult> OnPostDeleteAsync(int id)
    {
        var trainer = await dbContext.Trainers
            .Include(x => x.Reservations)
            .FirstOrDefaultAsync(x => x.Id == id);

        if (trainer is null)
        {
            return NotFound();
        }

        if (trainer.Reservations.Count > 0)
        {
            TempData["StatusMessage.Error"] = "This trainer cannot be deleted because reservations still exist.";
            return RedirectToPage();
        }

        dbContext.Trainers.Remove(trainer);
        await dbContext.SaveChangesAsync();

        logger.LogInformation("Trainer {TrainerId} deleted.", trainer.Id);
        TempData["StatusMessage.Success"] = "Trainer deleted.";
        return RedirectToPage();
    }

    public sealed record TrainerListItem(
        int Id,
        string FullName,
        string Specialty,
        decimal HourlyRate,
        bool IsActive,
        int ReservationCount);
}
