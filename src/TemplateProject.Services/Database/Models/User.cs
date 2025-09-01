using TemplateProject.Services.Enums;

namespace TemplateProject.Services.Database.Models;

/// <summary>
/// Represents a User record in the database.
/// </summary>
public class User
{
  /// <summary>
  /// The identifier associated with the user.
  /// </summary>
  public required int UserId { get; set; }

  /// <summary>
  /// The username of the user.
  /// </summary>
  public required string UserName { get; set; }

  /// <summary>
  /// The age of the user.
  /// </summary>
  public required int Age { get; set; }

  /// <summary>
  /// The pronouns of the user.
  /// </summary>
  public required Pronouns Pronouns { get; set; }

  /// <summary>
  /// The country of the user.
  /// </summary>
  public required string Country { get; set; }

  /// <summary>
  /// The email address of the user.
  /// </summary>
  public required string Email { get; set; }

  /// <summary>
  /// Indicates if the user is active.
  /// </summary>
  public required bool IsActive { get; set; }

  /// <summary>
  /// Optional social links for the user.
  /// </summary>
  public string? SocialLinks { get; set; }

  /// <summary>
  /// Tickets associated with the user.
  /// </summary>
  public ICollection<Ticket>? Tickets { get; set; }
}
