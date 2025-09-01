using System.Text.Json.Serialization;
using Asp.Versioning;
using Serilog;
using TemplateProject.API.Extensions;
using TemplateProject.API.Filters;
using TemplateProject.Services.Database;
using TemplateProject.Services.Extensions;
using TemplateProject.Services.Services;

namespace TemplateProject.API;

/// <summary>
/// Entry point for the API application.
/// </summary>
public static class Program
{
  /// <summary>
  /// Application entry point.
  /// </summary>
  /// <param name="args">Command line arguments.</param>
  /// <exception cref="InvalidOperationException">Thrown when the application configuration is invalid or incomplete during startup.</exception>
  private static void Main(string[] args)
  {
    var builder = WebApplication.CreateBuilder(args);
    var apiVersion = new ApiVersion(1);
    Log.Logger = new LoggerConfiguration()
      .WriteTo.Console(outputTemplate: "[{Timestamp:u} {Level}] {Message:lj}{NewLine}{Exception}")
      .CreateLogger();
    Log.Information("Starting up Template Project application.");

    try
    {
      // Register services
      builder.Services.AddControllers(config =>
        {
          config.Filters.Add<ApiKeyAuthorizationFilter>();
        })
        .AddJsonOptions(options =>
        {
          options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        })
        .AddXmlSerializerFormatters();
      builder.Services.AddProblemDetails();
      builder.Services.RegisterServices(builder.Configuration, builder.Environment);
      builder.Services.AddApiVersioningServices(apiVersion);
      builder.Services.AddEndpointsApiExplorer();
      builder.Services.ConfigureSwagger();

      // Logging is configured above with Serilog

      // Build the app
      var app = builder.Build();

      // Apply migration
      if (app.Environment.IsDevelopment())
      {
        MigrationRunner.ApplyMigrations(app.Services);
      }

      // Seed data if necessary
      using (var scope = app.Services.CreateScope())
      {
        var services = scope.ServiceProvider;
        SeedDataIfEmpty(services);
      }

      // Middleware
      app.UseStatusCodePages();
      app.UseExceptionHandler();
      app.UseSwagger(options => options.RouteTemplate = "openapi/{documentName}.json");
      app.ConfigureScalarApi();
      app.UseHttpsRedirection();
      app.MapControllers();

      // Run the app
      app.Run();
    }
    catch (Exception ex)
    {
      Log.Fatal(ex, "Unhandled exception happened while starting up Template Project application.");
    }
    finally
    {
      Log.Information("Template Project application shut down completed.");
      Log.CloseAndFlush();
    }
  }

  /// <summary>
  /// Seeds the database with initial data if no data exists.
  /// </summary>
  /// <param name="services">The service provider.</param>
  private static void SeedDataIfEmpty(IServiceProvider services)
  {
    // Replace 'YourDbContext' with your actual DbContext class name
    var dbContext = services.GetRequiredService<DatabaseContext>();

    // Seed Employees
    if (!dbContext.Employees.Any())
    {
      var employee = new Services.Database.Models.Employee
      {
        EmployeeId = 1,
        Name = "Alice Smith",
        Email = "alice.smith@example.com"
      };
      dbContext.Employees.Add(employee);
      dbContext.SaveChanges();
    }

    // Seed Users
    if (!dbContext.Users.Any())
    {
      var user = new Services.Database.Models.User
      {
        UserId = 1,
        UserName = "johndoe",
        Age = 30,
        Pronouns = Services.Enums.Pronouns.He,
        Country = "USA",
        Email = "john.doe@example.com",
        IsActive = true,
        SocialLinks = "https://github.com/johndoe"
      };
      dbContext.Users.Add(user);
      dbContext.SaveChanges();
    }

    // Seed Tickets
    if (!dbContext.Tickets.Any())
    {
      var user = dbContext.Users.First();
      var employee = dbContext.Employees.First();
      var ticket = new Services.Database.Models.Ticket
      {
        TicketId = 1,
        Subject = "Sample Ticket",
        Description = "This is a sample ticket.",
        CreatedAt = DateTime.UtcNow,
        IsActive = true,
        User = user,
        Status = Services.Enums.TicketStatus.Open,
        AssignedTo = employee
      };
      dbContext.Tickets.Add(ticket);
      dbContext.SaveChanges();
    }
  }
}
