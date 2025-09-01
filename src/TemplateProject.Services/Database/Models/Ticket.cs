using TemplateProject.Services.Enums;

namespace TemplateProject.Services.Database.Models;

/// <summary>
/// Represents a support ticket.
/// </summary>
public class Ticket
{
  /// <summary>
  /// Unique identifier for the ticket.
  /// </summary>
  public required int TicketId { get; set; }

  /// <summary>
  /// Subject of the ticket.
  /// </summary>
  public required string Subject { get; set; }

  /// <summary>
  /// Description of the ticket.
  /// </summary>
  public required string Description { get; set; }

  /// <summary>
  /// Date and time when the ticket was created.
  /// </summary>
  public required DateTime CreatedAt { get; set; }

  /// <summary>
  /// Date and time when the ticket was resolved (optional).
  /// </summary>
  public DateTime? ResolvedAt { get; set; }

  /// <summary>
  /// Indicates if the ticket is active.
  /// </summary>
  public required bool IsActive { get; set; }

  /// <summary>
  /// Reference to the user who owns the ticket.
  /// </summary>
  public required User User { get; set; }

  /// <summary>
  /// Status of the ticket.
  /// </summary>
  public required TicketStatus Status { get; set; }

  /// <summary>
  /// Reference to the employee assigned to the ticket.
  /// </summary>
  public Employee? AssignedTo { get; set; }
}
