using Microsoft.EntityFrameworkCore;
using ScheduleBotApi.Infrastructure.Contexts;

namespace ScheduleApi.Extensions;

public static class MigrationExtensions
{
    public static void ApplyMigrations(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var services = scope.ServiceProvider;

        try
        {
            var context = services.GetRequiredService<ScheduleContext>();

            if (context.Database.GetPendingMigrations().Any())
            {
                context.Database.Migrate();
                Console.WriteLine("Database migrations applied successfully.");
            }
            else
            {
                Console.WriteLine("Database is up to date. No migrations to apply.");
            }
        }
        catch (Exception ex)
        {
            var logger = services.GetRequiredService<ILogger<Program>>();
            logger.LogError(ex, "An error occurred while migrating the database.");
        }
    }
}