using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TemplateProject.Services.Dtos;
using TemplateProject.Services.Interfaces;
using TemplateProject.Services.Enums;
using TemplateProject.Services.Database;

namespace TemplateProject.Services.Services;

/// <summary>
/// Implementation of <see cref="IUserService"/> for managing and retrieving user information.
/// </summary>
public class UserService : IUserService
{
  private readonly IDbContextFactory<DatabaseContext> _dbContextFactory;
  private readonly ILogger<UserService> _logger;

  /// <summary>
  /// Initializes a new instance of the <see cref="UserService"/> class.
  /// </summary>
  /// <param name="dbContextFactory">A factory for creating instances of <see cref="DatabaseContext"/>. This is used to interact with the database.</param>
  /// <param name="logger">An instance of <see cref="ILogger{UserService}"/> used for logging information.</param>
  public UserService(IDbContextFactory<DatabaseContext> dbContextFactory, ILogger<UserService> logger)
  {
    _dbContextFactory = dbContextFactory;
    _logger = logger;
  }

  /// <inheritdoc />
  public async Task<UserDto?> GetUserById(int userId, CancellationToken token)
  {
    _logger.LogInformation("Retrieving user with ID: {UserId}", userId);
    await using var dbContext = _dbContextFactory.CreateDbContext();
    var user = await dbContext.Users
        .Include(u => u.Tickets)
        .FirstOrDefaultAsync(u => u.UserId == userId, cancellationToken: token);
    return user?.ToDto();
  }

  /// <inheritdoc />
  public async Task<UserDto?> GetUserByName(string userName, CancellationToken token)
  {
    _logger.LogInformation("Retrieving user with username: {UserName}", userName);
    await using var dbContext = _dbContextFactory.CreateDbContext();
    var user = await dbContext.Users
        .Include(u => u.Tickets)
        .FirstOrDefaultAsync(u => u.UserName == userName, cancellationToken: token);
    return user?.ToDto();
  }

  /// <inheritdoc />
  public async Task<IEnumerable<TicketDto>> GetTicketsForUser(int userId, CancellationToken token, TicketStatus? status = null, bool? isActive = null)
  {
    _logger.LogInformation("Retrieving tickets for user {UserId} with status: {Status} and isActive: {IsActive}", userId, status, isActive);
    await using var dbContext = _dbContextFactory.CreateDbContext();
    var query = dbContext.Tickets
        .Where(t => t.User.UserId == userId);

    if (status != null)
    {
      query = query.Where(t => t.Status == status);
    }

    if (isActive != null)
    {
      query = query.Where(t => t.User.IsActive == isActive);
    }

    var tickets = await query.ToListAsync(token);
    return tickets.Select(t => t.ToDto());
  }
}
