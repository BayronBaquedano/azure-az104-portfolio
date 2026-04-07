using FitAppReservations.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace FitAppReservations.Pages.Clients;

public class IndexModel(
    ApplicationDbContext dbContext,
    ILogger<IndexModel> logger) : PageModel
{
    [BindProperty(SupportsGet = true)]
    public string? Search { get; set; }

    public IReadOnlyList<ClientListItem> Clients { get; private set; } = Array.Empty<ClientListItem>();

    public async Task OnGetAsync()
    {
        var query = dbContext.Clients
            .AsNoTracking();

        if (!string.IsNullOrWhiteSpace(Search))
        {
            var searchTerm = Search.Trim();
            query = query.Where(x =>
                x.FullName.Contains(searchTerm) ||
                (x.Email ?? string.Empty).Contains(searchTerm) ||
                (x.Phone ?? string.Empty).Contains(searchTerm));
        }

        Clients = await query
            .OrderBy(x => x.FullName)
            .Select(x => new ClientListItem(
                x.Id,
                x.FullName,
                x.Email,
                x.Phone,
                x.Notes,
                dbContext.Reservations.Count(r => r.ClientId == x.Id)))
            .ToListAsync();
    }

    public async Task<IActionResult> OnPostDeleteAsync(int id)
    {
        var client = await dbContext.Clients
            .Include(x => x.Reservations)
            .FirstOrDefaultAsync(x => x.Id == id);

        if (client is null)
        {
            return NotFound();
        }

        if (client.Reservations.Count > 0)
        {
            TempData["StatusMessage.Error"] = "This client cannot be deleted because reservations still exist.";
            return RedirectToPage();
        }

        dbContext.Clients.Remove(client);
        await dbContext.SaveChangesAsync();

        logger.LogInformation("Client {ClientId} deleted.", client.Id);
        TempData["StatusMessage.Success"] = "Client deleted.";
        return RedirectToPage();
    }

    public sealed record ClientListItem(
        int Id,
        string FullName,
        string? Email,
        string? Phone,
        string? Notes,
        int ReservationCount);
}
