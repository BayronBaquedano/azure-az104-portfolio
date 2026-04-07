using System.ComponentModel.DataAnnotations;
using FitAppReservations.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace FitAppReservations.Pages.Trainers;

public class EditModel(
    ApplicationDbContext dbContext,
    ILogger<EditModel> logger) : PageModel
{
    [BindProperty]
    public InputModel Input { get; set; } = new();

    public async Task<IActionResult> OnGetAsync(int id)
    {
        var trainer = await dbContext.Trainers
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id);

        if (trainer is null)
        {
            return NotFound();
        }

        Input = new InputModel
        {
            FullName = trainer.FullName,
            Specialty = trainer.Specialty,
            HourlyRate = trainer.HourlyRate,
            IsActive = trainer.IsActive
        };

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int id)
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        var trainer = await dbContext.Trainers.FirstOrDefaultAsync(x => x.Id == id);
        if (trainer is null)
        {
            return NotFound();
        }

        trainer.FullName = Input.FullName;
        trainer.Specialty = Input.Specialty;
        trainer.HourlyRate = Input.HourlyRate;
        trainer.IsActive = Input.IsActive;

        await dbContext.SaveChangesAsync();

        logger.LogInformation("Trainer {TrainerId} updated.", trainer.Id);
        TempData["StatusMessage.Success"] = "Trainer updated.";
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
        public bool IsActive { get; set; }
    }
}
