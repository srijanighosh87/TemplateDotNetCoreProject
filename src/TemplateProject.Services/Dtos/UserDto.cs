using Riok.Mapperly.Abstractions;
using TemplateProject.Services.Database.Models;
using TemplateProject.Services.Enums;

namespace TemplateProject.Services.Dtos;

/// <summary>
/// Data transfer object for User entity.
/// </summary>
public class UserDto
{
  /// <summary>
  /// The identifier associated with the user.
  /// </summary>
  public int UserId { get; set; }

  /// <summary>
  /// The username of the user.
  /// </summary>
  public string UserName { get; set; } = string.Empty;

  /// <summary>
  /// The age of the user.
  /// </summary>
  public int Age { get; set; }

  /// <summary>
  /// The pronouns of the user.
  /// </summary>
  public Pronouns Pronouns { get; set; }

  /// <summary>
  /// The country of the user.
  /// </summary>
  public string Country { get; set; } = string.Empty;

  /// <summary>
  /// The email address of the user.
  /// </summary>
  public string Email { get; set; } = string.Empty;

  /// <summary>
  /// Indicates if the user is active.
  /// </summary>
  public bool IsActive { get; set; }

  /// <summary>
  /// Optional social links for the user.
  /// </summary>
  public string? SocialLinks { get; set; }

  /// <summary>
  /// List of ticket IDs associated with the user.
  /// </summary>
  public List<string>? TicketIds { get; set; }
}

/// <summary>
/// Provides mapping functionality between <see cref="User"/> and <see cref="UserDto"/> objects.
/// </summary>
[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public static partial class UserMapper
{
  /// <summary>
  /// Converts a <see cref="User"/> object to a <see cref="UserDto"/> object.
  /// </summary>
  /// <param name="user">The <see cref="User"/> instance to convert. Cannot be <see langword="null"/>.</param>
  [MapProperty(nameof(User.Tickets), nameof(UserDto.TicketIds), Use = nameof(MapTicketsToTicketIds))]
  public static partial UserDto ToDto(this User user);

  private static List<int>? MapTicketsToTicketIds(ICollection<Ticket>? tickets)
  {
    return tickets?.Select(t => t.TicketId).ToList();
  }
}
