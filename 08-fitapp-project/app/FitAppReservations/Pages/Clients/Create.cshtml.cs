using System.ComponentModel.DataAnnotations;
using FitAppReservations.Data;
using FitAppReservations.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FitAppReservations.Pages.Clients;

public class CreateModel(
    ApplicationDbContext dbContext,
    ILogger<CreateModel> logger) : PageModel
{
    [BindProperty]
    public InputModel Input { get; set; } = new();

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        var client = new Client
        {
            FullName = Input.FullName,
            Email = string.IsNullOrWhiteSpace(Input.Email) ? null : Input.Email.Trim(),
            Phone = string.IsNullOrWhiteSpace(Input.Phone) ? null : Input.Phone.Trim(),
            Notes = string.IsNullOrWhiteSpace(Input.Notes) ? null : Input.Notes.Trim()
        };

        dbContext.Clients.Add(client);
        await dbContext.SaveChangesAsync();

        logger.LogInformation("Client {ClientId} created.", client.Id);
        TempData["StatusMessage.Success"] = "Client created.";
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
