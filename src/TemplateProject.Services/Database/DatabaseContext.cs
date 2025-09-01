using Microsoft.EntityFrameworkCore;
using TemplateProject.Services.Database.Models;

namespace TemplateProject.Services.Database;

/// <summary>
/// Database context for the Server application.
/// </summary>
public class DatabaseContext : DbContext
{
  /// <summary>
  /// Initializes a new instance of the <see cref="DatabaseContext"/> class.
  /// </summary>
  /// <param name="options">The options for this context.</param>
  public DatabaseContext(DbContextOptions<DatabaseContext> options)
      : base(options)
  {
  }

  /// <summary>
  /// Gets or sets the DbSet for users.
  /// </summary>
  public DbSet<User> Users { get; set; }

  /// <summary>
  /// Gets or sets the DbSet for tickets.
  /// </summary>
  public DbSet<Ticket> Tickets { get; set; }

  /// <summary>
  /// Gets or sets the DbSet for employees.
  /// </summary>
  public DbSet<Employee> Employees { get; set; }

  /// <inheritdoc/>
  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    modelBuilder.HasDefaultSchema("TemplateProject");
  }

  /// <inheritdoc />
  protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
  {
    configurationBuilder.Properties<Enum>().HaveConversion<string>();
  }
}
