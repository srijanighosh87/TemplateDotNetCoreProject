using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TemplateProject.Services.Database;

namespace TemplateProject.Services.Services;

/// <summary>
/// Applies any pending database migrations for the application's database context.
/// </summary>
public class MigrationRunner
{
  /// <summary>
  /// Applies any pending database migrations for the application's database context.
  /// </summary>
  /// <param name="serviceProvider">The service provider.</param>
  public static void ApplyMigrations(IServiceProvider serviceProvider)
  {
    using var scope = serviceProvider.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<MigrationRunner>>();

    var pendingMigrations = dbContext.Database.GetPendingMigrations();
    if (pendingMigrations.FirstOrDefault() is { } firstMigration)
    {
      dbContext.Database.Migrate();
      var allMigrations = dbContext.Database.GetMigrations();
      if (firstMigration == allMigrations.First())
      {
        logger.LogInformation("The database has been created successfully.");
      }
      else
      {
        logger.LogInformation("The database has been updated successfully.");
      }
    }
  }
}
