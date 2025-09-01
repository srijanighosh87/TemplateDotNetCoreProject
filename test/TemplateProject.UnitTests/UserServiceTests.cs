using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NSubstitute;
using TemplateProject.Services.Database;
using TemplateProject.Services.Database.Models;
using TemplateProject.Services.Dtos;
using TemplateProject.Services.Services;
using Xunit;
using TemplateProject.Services.Enums;

namespace TemplateProject.UnitTests;

/// <summary>
/// Contains unit tests for the <see cref="UserService"/> class, verifying its behavior when retrieving user data by ID
/// and name.
/// </summary>
public class UserServiceTests
{
  /// <summary>
  /// Tests that the <see cref="UserService.GetUserById"/> method returns a <see cref="UserDto"/> object when a user
  /// with the specified ID exists in the database.
  /// </summary>
  [Fact]
  public async Task GetUserById_ReturnsUserDto_WhenUserExists()
  {
    // Arrange
    await using var dbContext = GetDbContext();
    await SeedDataAsync(dbContext);
    var service = CreateService(dbContext);

    // Act
    var result = await service.GetUserById(1, TestContext.Current.CancellationToken);

    // Assert
    result.Should().NotBeNull()
      .And.BeOfType<UserDto>()
      .And.Match<UserDto>(u => u.UserId == 1 && u.UserName == "alice" && u.Email == "alice@wonderland.com");
  }

  /// <summary>
  /// Tests that <see cref="UserService.GetUserById"/> returns <see langword="null"/> when the specified user ID does
  /// not exist.
  /// </summary>
  [Fact]
  public async Task GetUserById_ReturnsNull_WhenUserDoesNotExist()
  {
    // Arrange
    await using var dbContext = GetDbContext();
    var service = CreateService(dbContext);

    // Act
    var result = await service.GetUserById(1000000, TestContext.Current.CancellationToken);

    // Assert
    result.Should().BeNull();
  }

  /// <summary>
  /// Tests that the <see cref="UserService.GetUserByName"/> method returns a <see cref="UserDto"/> when a user with the
  /// specified name exists in the database.
  /// </summary>
  [Fact]
  public async Task GetUserByName_ReturnsUserDto_WhenUserExists()
  {
    // Arrange
    await using var dbContext = GetDbContext();
    await SeedDataAsync(dbContext);
    var service = CreateService(dbContext);

    // Act
    var result = await service.GetUserByName("bob", TestContext.Current.CancellationToken);

    // Assert
    result.Should().NotBeNull()
      .And.BeOfType<UserDto>()
      .And.Match<UserDto>(u => u.UserId == 2 && u.UserName == "bob" && u.Email == "bob@builder.com");
  }

  /// <summary>
  /// Tests that <see cref="UserService.GetUserByName(string, CancellationToken)"/> returns <see langword="null"/> when the specified user
  /// does not exist.
  /// </summary>
  [Fact]
  public async Task GetUserByName_ReturnsNull_WhenUserDoesNotExist()
  {
    // Arrange
    await using var dbContext = GetDbContext();
    var service = CreateService(dbContext);

    // Act
    var result = await service.GetUserByName("NON_EXISTENT_USERNAME", TestContext.Current.CancellationToken);

    // Assert
    result.Should().BeNull();
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
    await dbContext.Users.AddRangeAsync(
    [
      new User
      {
          UserId = 1,
          UserName = "alice",
          Age = 30,
          Pronouns = Pronouns.She,
          Country = "Wonderland",
          Email = "alice@wonderland.com",
          IsActive = true,
          Tickets = []
      },
      new User
      {
          UserId = 2,
          UserName = "bob",
          Age = 25,
          Pronouns = Pronouns.He,
          Country = "Builderland",
          Email = "bob@builder.com",
          IsActive = false,
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
  private static UserService CreateService(DatabaseContext dbContext)
  {
    var dbContextFactory = Substitute.For<IDbContextFactory<DatabaseContext>>();
    dbContextFactory.CreateDbContext().Returns(dbContext);
    var logger = Substitute.For<ILogger<UserService>>();
    return new UserService(dbContextFactory, logger);
  }
}
