using Contracts;
using Microsoft.EntityFrameworkCore;

namespace API.Extensions;

public static class MigrationExtensions
{
    public static async Task ApplyMigrationsAsync<TContext>(this IApplicationBuilder app)
   where TContext : DbContext
    {
        using var scope = app.ApplicationServices.CreateScope();
        var logger = app.ApplicationServices.GetRequiredService<ILoggerManager>();

        var dbContext = scope.ServiceProvider.GetRequiredService<TContext>();

        try
        {
            logger.LogInfo($"Starting database migration for {typeof(TContext).Name}");
            await dbContext.Database.MigrateAsync();
            logger.LogInfo($"Database migration completed for {typeof(TContext).Name}");
        }
        catch (Exception ex)
        {
            logger.LogError($"An error occurred while migrating the database for {typeof(TContext).Name} , {ex}");
            throw;
        }
    }
}
