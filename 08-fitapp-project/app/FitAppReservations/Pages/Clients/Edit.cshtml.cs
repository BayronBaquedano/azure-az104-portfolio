using System.ComponentModel.DataAnnotations;
using FitAppReservations.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace FitAppReservations.Pages.Clients;

public class EditModel(
    ApplicationDbContext dbContext,
    ILogger<EditModel> logger) : PageModel
{
    [BindProperty]
    public InputModel Input { get; set; } = new();

    public async Task<IActionResult> OnGetAsync(int id)
    {
        var client = await dbContext.Clients
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id);

        if (client is null)
        {
            return NotFound();
        }

        Input = new InputModel
        {
            FullName = client.FullName,
            Email = client.Email,
            Phone = client.Phone,
            Notes = client.Notes
        };

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int id)
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        var client = await dbContext.Clients.FirstOrDefaultAsync(x => x.Id == id);
        if (client is null)
        {
            return NotFound();
        }

        client.FullName = Input.FullName;
        client.Email = string.IsNullOrWhiteSpace(Input.Email) ? null : Input.Email.Trim();
        client.Phone = string.IsNullOrWhiteSpace(Input.Phone) ? null : Input.Phone.Trim();
        client.Notes = string.IsNullOrWhiteSpace(Input.Notes) ? null : Input.Notes.Trim();

        await dbContext.SaveChangesAsync();

        logger.LogInformation("Client {ClientId} updated.", client.Id);
        TempData["StatusMessage.Success"] = "Client updated.";
        return RedirectToPage("Index");
    }

    public class InputModel
    {
        [Required]
        [StringLength(120)]
        [Display(Name = "Full name")]
        public string FullName { get; set; } = string.Empty;

        [EmailAddress]
        [StringLength(200)]
        public string? Email { get; set; }

        [StringLength(50)]
        public string? Phone { get; set; }

        [StringLength(1000)]
        public string? Notes { get; set; }
    }
}
