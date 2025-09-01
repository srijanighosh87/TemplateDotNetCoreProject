using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TemplateProject.Services.Dtos;
using TemplateProject.Services.Interfaces;
using TemplateProject.Services.Extensions;
using TemplateProject.Services.Database.Models;
using TemplateProject.Services.Enums;
using TemplateProject.Services.Database;

namespace TemplateProject.Services.Services;

/// <summary>
/// Implementation of <see cref="ITicketService"/> for managing and retrieving ticket information.
/// </summary>
public class TicketService : ITicketService
{
  private readonly IDbContextFactory<DatabaseContext> _dbContextFactory;
  private readonly ILogger<TicketService> _logger;

  /// <summary>
  /// Initializes a new instance of the <see cref="TicketService"/> class.
  /// </summary>
  /// <param name="dbContextFactory">The factory used to create instances of <see cref="DatabaseContext"/> for database operations.</param>
  /// <param name="logger">The logger.</param>
  public TicketService(IDbContextFactory<DatabaseContext> dbContextFactory, ILogger<TicketService> logger)
  {
    _dbContextFactory = dbContextFactory;
    _logger = logger;
  }

  ///<inheritdoc/>
  public async Task<TicketDto?> GetTicketById(int ticketId, CancellationToken token)
  {
    _logger.LogInformation("Retrieving ticket with ID: {TicketId}", ticketId);
    await using var dbContext = _dbContextFactory.CreateDbContext();
    var ticket = await dbContext.Tickets
        .Include(t => t.User)
        .Include(t => t.AssignedTo)
        .FirstOrDefaultAsync(t => t.TicketId == ticketId, token);
    return ticket?.ToDto();
  }

  ///<inheritdoc/>
  public async Task<IEnumerable<TicketDto>> GetTickets(
      int? userId = null,
      DateTime? createdBefore = null,
      DateTime? createdAfter = null,
      int? assignedToEmployeeId = null,
      int? resultCount = 20,
      CancellationToken token = default)
  {
    _logger.LogInformation("Retrieving tickets with filters - UserId: {UserId}, CreatedBefore: {CreatedBefore}, CreatedAfter: {CreatedAfter}, AssignedToEmployeeId: {AssignedToEmployeeId}, ResultCount: {ResultCount}",
        userId, createdBefore, createdAfter, assignedToEmployeeId, resultCount);
    await using var dbContext = _dbContextFactory.CreateDbContext();
    var query = dbContext.Tickets
        .Include(t => t.User)
        .Include(t => t.AssignedTo)
        .AsQueryable()
        .WhereIf(userId != null, t => t.User.UserId == userId)
        .WhereIf(createdBefore != null, t => t.CreatedAt < createdBefore)
        .WhereIf(createdAfter != null, t => t.CreatedAt > createdAfter)
        .WhereIf(assignedToEmployeeId != null, t => t.AssignedTo != null && t.AssignedTo.EmployeeId == assignedToEmployeeId);

    if (resultCount is not null and > 0)
    {
      query = query.Take(resultCount.Value);
    }

    var tickets = await query.ToListAsync(token);
    return tickets.Select(t => t.ToDto());
  }

  ///<inheritdoc/>
  public async Task<int?> CreateTicket(int userId, string subject, string description, CancellationToken token)
  {
    _logger.LogInformation("Creating ticket for user {UserId} with subject: {Subject}", userId, subject);
    await using var dbContext = _dbContextFactory.CreateDbContext();
    var user = await dbContext.Users.FirstOrDefaultAsync(u => u.UserId == userId, token);
    if (user == null)
    {
      _logger.LogWarning("User with id {UserId} not found. Ticket not created.", userId);
      return null;
    }

    var ticket = new Ticket
    {
      TicketId = 0, // Assuming 0 lets the database auto-generate the ID; adjust if needed
      Subject = subject,
      Description = description,
      CreatedAt = DateTime.UtcNow,
      IsActive = true,
      Status = TicketStatus.Open,
      User = user
    };
    dbContext.Tickets.Add(ticket);
    await dbContext.SaveChangesAsync(token);
    return ticket.TicketId;
  }
}
