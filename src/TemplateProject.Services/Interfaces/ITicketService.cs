using TemplateProject.Services.Dtos;

namespace TemplateProject.Services.Interfaces;

/// <summary>
/// Interface for managing and retrieving ticket information.
/// </summary>
public interface ITicketService
{
  /// <summary>
  /// Gets a ticket by its unique identifier.
  /// </summary>
  /// <param name="ticketId">The unique identifier of the ticket.</param>
  /// <param name="token">Cancellation token for the request.</param>
  /// <returns>The ticket details if found; otherwise, null.</returns>
  Task<TicketDto?> GetTicketById(int ticketId, CancellationToken token);

  /// <summary>
  /// Gets tickets filtered by user who created the ticket, by date (before or after), or by employee id to whom the ticket is assigned.
  /// </summary>
  /// <param name="userId">Optional user id who created the ticket.</param>
  /// <param name="createdBefore">Optional filter for tickets created before this date.</param>
  /// <param name="createdAfter">Optional filter for tickets created after this date.</param>
  /// <param name="assignedToEmployeeId">Optional employee id to whom the ticket is assigned.</param>
  /// <param name="resultCount">Optional number of results to return. Default value is 20.</param>
  /// <param name="token">Cancellation token for the request.</param>
  /// <returns>A list of filtered ticket DTOs.</returns>
  Task<IEnumerable<TicketDto>> GetTickets(
      int? userId = null,
      DateTime? createdBefore = null,
      DateTime? createdAfter = null,
      int? assignedToEmployeeId = null,
      int? resultCount = null,
      CancellationToken token = default);

  /// <summary>
  /// Creates a new ticket for a user with the specified subject and description.
  /// </summary>
  /// <param name="userId">The unique identifier of the user who creates the ticket.</param>
  /// <param name="subject">The subject of the ticket.</param>
  /// <param name="description">The description of the ticket.</param>
  /// <param name="token">Cancellation token for the request.</param>
  /// <returns>The created ticket ID if successful; otherwise, null.</returns>
  Task<int?> CreateTicket(int userId, string subject, string description, CancellationToken token);
}
