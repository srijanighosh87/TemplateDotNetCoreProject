using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NSubstitute;
using TemplateProject.Services.Database;
using TemplateProject.Services.Database.Models;
using TemplateProject.Services.Dtos;
using TemplateProject.Services.Services;
using TemplateProject.Services.Enums;
using Xunit;

namespace TemplateProject.UnitTests;

/// <summary>
/// Contains unit tests for the <see cref="TicketService"/> class, verifying its behavior in various scenarios.
/// </summary>
public class TicketServiceTests
{
  /// <summary>
  /// Tests that the <see cref="TicketService.GetTicketById"/> method returns a <see cref="TicketDto"/> when a ticket
  /// with the specified ID exists in the database.
  /// </summary>
  [Fact]
  public async Task GetTicketById_ReturnsTicketDto_WhenTicketExists()
  {
    // Arrange
    var (service, dbContext) = CreateServiceWithTestDbContext();
    await SeedDataAsync(dbContext);

    // Act
    var result = await service.GetTicketById(1, CancellationToken.None);

    // Assert
    result.Should().NotBeNull()
        .And.BeOfType<TicketDto>()
        .And.Match<TicketDto>(t => t.TicketId == 1 && t.Subject == "Login Issue");
  }

  /// <summary>
  /// Tests that <see cref="TicketService.GetTicketById"/> returns <see langword="null"/> when a ticket with the specified ID does not
  /// exist.
  /// </summary>
  [Fact]
  public async Task GetTicketById_ReturnsNull_WhenTicketDoesNotExist()
  {
    // Arrange
    var (service, dbContext) = CreateServiceWithTestDbContext();
    await SeedDataAsync(dbContext);

    // Act
    var result = await service.GetTicketById(10000, CancellationToken.None);

    // Assert
    result.Should().BeNull();
  }

  /// <summary>
  /// Tests that the <see cref="TicketService.GetTickets"/> method returns all ticket DTOs for a specified user.
  /// </summary>
  [Fact]
  public async Task GetTickets_ReturnsAllTicketDtos()
  {
    // Arrange
    var (service, dbContext) = CreateServiceWithTestDbContext();
    await SeedDataAsync(dbContext);

    // Act
    var result = await service.GetTickets(userId: 1, token: CancellationToken.None);

    // Assert
    var list = new List<TicketDto>(result);
    list.Count.Should().Be(2);
    list.Should().Contain(t => t.TicketId == 1 && t.Subject == "Login Issue");
    list.Should().Contain(t => t.TicketId == 2 && t.Subject == "Payment Problem");
  }

  /// <summary>
  /// Tests that the <see cref="TicketService.CreateTicket"/> method creates a new ticket and returns a valid ticket ID when the user
  /// exists.
  /// </summary>
  [Fact]
  public async Task CreateTicket_CreatesAndReturnsTicketId_WhenUserExists()
  {
    // Arrange
    var (service, dbContext) = CreateServiceWithTestDbContext();
    await SeedDataAsync(dbContext);

    // Act
    var ticketId = await service.CreateTicket(1, "New Issue", "Details here", CancellationToken.None);

    // Assert
    ticketId.Should().NotBeNull();
    var created = await dbContext.Tickets.FirstOrDefaultAsync(t => t.TicketId == ticketId, TestContext.Current.CancellationToken);
    created.Should().NotBeNull();
    created!.Subject.Should().Be("New Issue");
    created.Description.Should().Be("Details here");
  }

  /// <summary>
  /// Tests that the <c>CreateTicket</c> method returns <see langword="null"/> when the specified user does not exist.
  /// </summary>
  [Fact]
  public async Task CreateTicket_ReturnsNull_WhenUserDoesNotExist()
  {
    // Arrange
    var (service, dbContext) = CreateServiceWithTestDbContext();
    await SeedDataAsync(dbContext);

    // Act
    var ticketId = await service.CreateTicket(100000, "Subject", "Description", CancellationToken.None);

    // Assert
    ticketId.Should().BeNull();
  }

  /// <summary>
  /// Asynchronously seeds initial data into the specified database context.
  /// </summary>
  /// <param name="dbContext">The database context to seed with initial data. Must not be null.</param>
  private static async Task SeedDataAsync(DatabaseContext dbContext)
  {
    var user = new User
    {
      UserId = 1,
      UserName = "alice",
      Age = 30,
      Pronouns = Pronouns.She,
      Country = "Wonderland",
      Email = "alice@wonderland.com",
      IsActive = true,
      Tickets = []
    };
    dbContext.Users.Add(user);
    await dbContext.SaveChangesAsync();
    dbContext.Tickets.AddRange(
        new Ticket
        {
          TicketId = 1,
          Subject = "Login Issue",
          Description = "Cannot login to account.",
          CreatedAt = DateTime.UtcNow.AddDays(-1),
          IsActive = true,
          Status = TicketStatus.Open,
          User = user
        },
        new Ticket
        {
          TicketId = 2,
          Subject = "Payment Problem",
          Description = "Payment not processed.",
          CreatedAt = DateTime.UtcNow,
          IsActive = true,
          Status = TicketStatus.InProgress,
          User = user
        }
    );
    await dbContext.SaveChangesAsync();
  }

  /// <summary>
  /// Creates a TicketService using a shared in-memory database.
  /// Returns the service and a test DbContext for seeding/assertions.
  /// </summary>
  private static (TicketService Service, DatabaseContext TestDbContext) CreateServiceWithTestDbContext()
  {
    var options = new DbContextOptionsBuilder<DatabaseContext>()
        .UseInMemoryDatabase(databaseName: "TicketServiceTestDb" + Guid.NewGuid())
        .Options;

    var testDbContext = new DatabaseContext(options);
    testDbContext.Database.EnsureCreated();
    var dbContextFactory = Substitute.For<IDbContextFactory<DatabaseContext>>();
    dbContextFactory.CreateDbContext().Returns(_ => new DatabaseContext(options));

    var logger = Substitute.For<ILogger<TicketService>>();
    var service = new TicketService(dbContextFactory, logger);

    return (service, testDbContext);
  }
}
