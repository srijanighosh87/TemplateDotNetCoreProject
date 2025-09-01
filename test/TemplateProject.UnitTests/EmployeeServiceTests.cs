using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NSubstitute;
using TemplateProject.Services.Database;
using TemplateProject.Services.Database.Models;
using TemplateProject.Services.Dtos;
using TemplateProject.Services.Services;
using Xunit;

namespace TemplateProject.UnitTests;

/// <summary>
/// Contains unit tests for the <see cref="EmployeeService"/> class, verifying its behavior in various scenarios.
/// </summary>
public class EmployeeServiceTests
{
  /// <summary>
  /// Tests that the <see cref="EmployeeService.GetEmployeeById"/> method returns an <see cref="EmployeeDto"/> when an
  /// employee with the specified ID exists in the database.
  /// </summary>
  [Fact]
  public async Task GetEmployeeById_ReturnsEmployeeDto_WhenEmployeeExists()
  {
    // Arrange
    await using var dbContext = GetDbContext();
    await SeedDataAsync(dbContext);
    var service = CreateService(dbContext);

    // Act
    var result = await service.GetEmployeeById(1, TestContext.Current.CancellationToken);

    // Assert
    result.Should().NotBeNull()
      .And.BeOfType<EmployeeDto>()
      .And.Match<EmployeeDto>(e => e.EmployeeId == 1 && e.Name == "John Doe" && e.Email == "john.doe@example.com");
  }

  /// <summary>
  /// Tests that <see cref="EmployeeService.GetEmployeeById"/> returns <see langword="null"/> when the specified
  /// employee ID does not exist.
  /// </summary>
  [Fact]
  public async Task GetEmployeeById_ReturnsNull_WhenEmployeeDoesNotExist()
  {
    // Arrange
    await using var dbContext = GetDbContext();
    var service = CreateService(dbContext);

    // Act
    var result = await service.GetEmployeeById(10000000, TestContext.Current.CancellationToken);

    // Assert
    result.Should().BeNull();
  }

  /// <summary>
  /// Tests that the <see cref="EmployeeService.GetAllEmployees"/> method returns all employee DTOs from the database.
  /// </summary>
  [Fact]
  public async Task GetAllEmployees_ReturnsAllEmployeeDtos()
  {
    // Arrange
    await using var dbContext = GetDbContext();
    await SeedDataAsync(dbContext);
    var service = CreateService(dbContext);

    // Act
    var result = await service.GetAllEmployees(CancellationToken.None);

    // Assert
    var list = new List<EmployeeDto>(result);
    list.Count.Should().Be(2);
    list.Should().Contain(e => e.EmployeeId == 1 && e.Name == "John Doe");
    list.Should().Contain(e => e.EmployeeId == 2 && e.Name == "Jane Smith");
  }

  /// <summary>
  /// Creates and returns a new instance of <see cref="DatabaseContext"/> configured to use an in-memory database.
  /// </summary>
  private static DatabaseContext GetDbContext()
  {
    var options = new DbContextOptionsBuilder<DatabaseContext>()
        .UseInMemoryDatabase(Guid.NewGuid().ToString())
        .Options;

    var context = new DatabaseContext(options);
    context.Database.EnsureCreated();
    return context;
  }

  /// <summary>
  /// Populates the database with initial employee data.
  /// </summary>
  /// <param name="dbContext">The database context used to access the data store. Must not be <see langword="null"/>.</param>
  private static async Task SeedDataAsync(DatabaseContext dbContext)
  {
    await dbContext.Employees.AddRangeAsync(
    [
      new Employee
      {
          EmployeeId = 1,
          Name = "John Doe",
          Email = "john.doe@example.com",
          Tickets = []
      },
      new Employee
      {
          EmployeeId = 2,
          Name = "Jane Smith",
          Email = "jane.smith@example.com",
          Tickets = []
      }
    ]);
    await dbContext.SaveChangesAsync();
  }

  /// <summary>
  /// Creates a mocked EmployeeService for unit testing.
  /// </summary>
  /// <param name="dbContext">The database context to use.</param>
  /// <returns>An instance of EmployeeService with mocked dependencies.</returns>
  private static EmployeeService CreateService(DatabaseContext dbContext)
  {
    var dbContextFactory = Substitute.For<IDbContextFactory<DatabaseContext>>();
    dbContextFactory.CreateDbContext().Returns(dbContext);
    var logger = Substitute.For<ILogger<EmployeeService>>();
    return new EmployeeService(dbContextFactory, logger);
  }
}
