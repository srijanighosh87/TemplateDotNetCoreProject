using System.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TemplateProject.Services.Database;
using TemplateProject.Services.Interfaces;
using TemplateProject.Services.Services;

namespace TemplateProject.Services.Extensions;

/// <summary>
/// Extension methods for configuring application services.
/// </summary>
public static class ServiceConfiguration
{
  /// <summary>
  /// Registers application services with the dependency injection container.
  /// </summary>
  /// <param name="services">The service collection to configure.</param>
  /// <param name="configuration">The application configuration instance.</param>
  /// <param name="environment">Hosting environment.</param>
  /// <returns>The service collection for method chaining.</returns>
  public static IServiceCollection RegisterServices(this IServiceCollection services, IConfiguration configuration, IHostEnvironment environment)
  {
    // Register application services
    services.AddScoped<IEmployeeService, EmployeeService>();
    services.AddScoped<ITicketService, TicketService>();
    services.AddScoped<IUserService, UserService>();
    services.AddScoped<IApiKeyValidationService, ApiKeyValidationService>();

    // Register database services
    services.RegisterDbServices(configuration, environment.IsDevelopment());

    // Add other services here as needed
    return services;
  }

  /// <summary>
  /// Registers database services with the dependency injection container.
  /// </summary>
  /// <param name="services">The service collection to configure.</param>
  /// <param name="configuration">The configuration to use.</param>
  /// <param name="isDevelopment">Value is true, if the environment is Development.</param>
  /// <returns>The service collection for method chaining.</returns>
  private static IServiceCollection RegisterDbServices(this IServiceCollection services, IConfiguration configuration, bool isDevelopment)
  {
    services.AddDbContextFactory<DatabaseContext>(options =>
    {
      options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"))
        .LogTo(s => Debug.WriteLine(s), LogLevel.Information);
      if (isDevelopment)
      {
        options.EnableDetailedErrors()
          .EnableSensitiveDataLogging();
      }
    });

    return services;
  }
}
