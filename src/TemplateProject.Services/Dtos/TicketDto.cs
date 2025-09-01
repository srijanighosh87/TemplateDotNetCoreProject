using Riok.Mapperly.Abstractions;
using TemplateProject.Services.Database.Models;
using TemplateProject.Services.Enums;

namespace TemplateProject.Services.Dtos;

/// <summary>
/// Data transfer object for Ticket entity.
/// </summary>
public class TicketDto
{
  /// <summary>
  /// Unique identifier for the ticket.
  /// </summary>
  public int TicketId { get; set; }

  /// <summary>
  /// Subject of the ticket.
  /// </summary>
  public string Subject { get; set; } = string.Empty;

  /// <summary>
  /// Description of the ticket.
  /// </summary>
  public string Description { get; set; } = string.Empty;

  /// <summary>
  /// Date and time when the ticket was created.
  /// </summary>
  public DateTime CreatedAt { get; set; }

  /// <summary>
  /// Date and time when the ticket was resolved (optional).
  /// </summary>
  public DateTime? ResolvedAt { get; set; }

  /// <summary>
  /// Indicates if the ticket is active.
  /// </summary>
  public bool IsActive { get; set; }

  /// <summary>
  /// Status of the ticket.
  /// </summary>
  public TicketStatus Status { get; set; }

  /// <summary>
  /// Identifier of the employee assigned to the ticket (optional).
  /// </summary>
  public string? AssignedToEmployeeId { get; set; }
}

/// <summary>
/// Provides mapping functionality for converting <see cref="Ticket"/> objects to <see cref="TicketDto"/> objects.
/// </summary>
[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public static partial class TicketMapper
{
  /// <summary>
  /// Converts a <see cref="Ticket"/> object to a <see cref="TicketDto"/> object.
  /// </summary>
  /// <param name="ticket">The <see cref="Ticket"/> instance to convert. Cannot be <see langword="null"/>.</param>
  public static partial TicketDto ToDto(this Ticket ticket);
}
