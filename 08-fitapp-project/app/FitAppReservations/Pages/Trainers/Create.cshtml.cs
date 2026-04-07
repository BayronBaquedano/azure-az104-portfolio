using System.ComponentModel.DataAnnotations;
using FitAppReservations.Data;
using FitAppReservations.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FitAppReservations.Pages.Trainers;

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

        var trainer = new Trainer
        {
            FullName = Input.FullName,
            Specialty = Input.Specialty,
            HourlyRate = Input.HourlyRate,
            IsActive = Input.IsActive
        };

        dbContext.Trainers.Add(trainer);
        await dbContext.SaveChangesAsync();

        logger.LogInformation("Trainer {TrainerId} created.", trainer.Id);
        TempData["StatusMessage.Success"] = "Trainer created.";
        return RedirectToPage("Index");
    }

    public class InputModel
    {
        [Required]
        [StringLength(120)]
        [Display(Name = "Full name")]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [StringLength(120)]
        public string Specialty { get; set; } = string.Empty;

        [Range(typeof(decimal), "0", "99999999.99")]
        [Display(Name = "Hourly rate")]
        public decimal HourlyRate { get; set; }

        [Display(Name = "Active")]
        public bool IsActive { get; set; } = true;
    }
}
