using FitAppReservations.Models;
using Microsoft.EntityFrameworkCore;

namespace FitAppReservations.Data;

public static class ApplicationDbInitializer
{
    public static async Task InitializeAsync(WebApplication app)
    {
        await using var scope = app.Services.CreateAsyncScope();
        var services = scope.ServiceProvider;
        var logger = services.GetRequiredService<ILoggerFactory>()
            .CreateLogger("ApplicationDbInitializer");
        var configuration = services.GetRequiredService<IConfiguration>();
        var environment = services.GetRequiredService<IHostEnvironment>();
        var dbContext = services.GetRequiredService<ApplicationDbContext>();

        var applyMigrationsOnStartup = configuration.GetValue<bool?>("Database:ApplyMigrationsOnStartup")
            ?? environment.IsDevelopment();

        try
        {
            if (applyMigrationsOnStartup)
            {
                logger.LogInformation("Applying database migrations on startup.");
                await dbContext.Database.MigrateAsync();
            }
            else
            {
                var pendingMigrations = await dbContext.Database.GetPendingMigrationsAsync();
                if (pendingMigrations.Any())
                {
                    logger.LogWarning(
                        "Database has pending migrations. Set Database__ApplyMigrationsOnStartup=true or run migrations manually.");
                }
            }

            if (await dbContext.Trainers.AnyAsync())
            {
                logger.LogInformation("Database already contains seed data. Skipping sample seed.");
                return;
            }

            var trainers = new[]
            {
                new Trainer
                {
                    FullName = "Sofia Alvarez",
                    Specialty = "Strength Training",
                    HourlyRate = 55.00m,
                    IsActive = true
                },
                new Trainer
                {
                    FullName = "Daniel Reed",
                    Specialty = "Mobility and Recovery",
                    HourlyRate = 48.50m,
                    IsActive = true
                },
                new Trainer
                {
                    FullName = "Maya Thompson",
                    Specialty = "Weight Loss Coaching",
                    HourlyRate = 62.00m,
                    IsActive = false
                }
            };

            var clients = new[]
            {
                new Client
                {
                    FullName = "Liam Carter",
                    Email = "liam.carter@example.com",
                    Phone = "+1 555-100-1001",
                    Notes = "Prefers morning sessions."
                },
                new Client
                {
                    FullName = "Emma Brooks",
                    Email = "emma.brooks@example.com",
                    Phone = "+1 555-100-1002",
                    Notes = "Needs low-impact workouts."
                },
                new Client
                {
                    FullName = "Noah Wilson",
                    Email = "noah.wilson@example.com",
                    Phone = "+1 555-100-1003",
                    Notes = "Training for a half marathon."
                }
            };

            await dbContext.Trainers.AddRangeAsync(trainers);
            await dbContext.Clients.AddRangeAsync(clients);
            await dbContext.SaveChangesAsync();

            var today = DateOnly.FromDateTime(DateTime.Today);
            var reservations = new[]
            {
                new Reservation
                {
                    TrainerId = trainers[0].Id,
                    ClientId = clients[0].Id,
                    ReservationDate = today.AddDays(1),
                    StartTime = new TimeOnly(9, 0),
                    EndTime = new TimeOnly(10, 0),
                    Status = ReservationStatus.Confirmed,
                    Notes = "Intro strength plan."
                },
                new Reservation
                {
                    TrainerId = trainers[0].Id,
                    ClientId = clients[1].Id,
                    ReservationDate = today.AddDays(1),
                    StartTime = new TimeOnly(11, 0),
                    EndTime = new TimeOnly(12, 0),
                    Status = ReservationStatus.Pending,
                    Notes = "Follow-up evaluation."
                },
                new Reservation
                {
                    TrainerId = trainers[1].Id,
                    ClientId = clients[2].Id,
                    ReservationDate = today.AddDays(2),
                    StartTime = new TimeOnly(16, 0),
                    EndTime = new TimeOnly(17, 0),
                    Status = ReservationStatus.Confirmed,
                    Notes = "Mobility session."
                },
                new Reservation
                {
                    TrainerId = trainers[1].Id,
                    ClientId = clients[0].Id,
                    ReservationDate = today.AddDays(3),
                    StartTime = new TimeOnly(14, 30),
                    EndTime = new TimeOnly(15, 30),
                    Status = ReservationStatus.Cancelled,
                    Notes = "Cancelled by client."
                }
            };

            await dbContext.Reservations.AddRangeAsync(reservations);
            await dbContext.SaveChangesAsync();

            logger.LogInformation("Seeded sample trainers, clients, and reservations.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while initializing the database.");
            throw;
        }
    }
}
