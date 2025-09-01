using TemplateProject.Services.Dtos;
using TemplateProject.Services.Enums;

namespace TemplateProject.Services.Interfaces;

/// <summary>
/// Interface for managing and retrieving user information.
/// </summary>
public interface IUserService
{
  /// <summary>
  /// Gets a user by their unique identifier.
  /// </summary>
  /// <param name="userId">The unique identifier of the user.</param>
  /// <param name="token">The cancellation token.</param>
  Task<UserDto?> GetUserById(int userId, CancellationToken token);

  /// <summary>
  /// Gets a user by their username.
  /// </summary>
  /// <param name="userName">The username of the user.</param>
  /// <param name="token">The cancellation token.</param>
  Task<UserDto?> GetUserByName(string userName, CancellationToken token);

  /// <summary>
  /// Gets tickets for a user, filtered by ticket status and user IsActive.
  /// </summary>
  /// <param name="userId">The unique identifier of the user.</param>
  /// <param name="token">The cancellation token.</param>
  /// <param name="status">Optional ticket status filter.</param>
  /// <param name="isActive">Optional user IsActive filter.</param>
  Task<IEnumerable<TicketDto>> GetTicketsForUser(int userId, CancellationToken token, TicketStatus? status = null, bool? isActive = null);
}
