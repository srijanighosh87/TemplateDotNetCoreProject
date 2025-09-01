namespace TemplateProject.Services.Database.Models;

/// <summary>
/// Represents an employee who can be assigned to tickets.
/// </summary>
public class Employee
{
  /// <summary>
  /// Unique identifier for the employee.
  /// </summary>
  public required int EmployeeId { get; set; }

  /// <summary>
  /// Name of the employee.
  /// </summary>
  public required string Name { get; set; }

  /// <summary>
  /// Email address of the employee.
  /// </summary>
  public required string Email { get; set; }

  /// <summary>
  /// Tickets assigned to the employee.
  /// </summary>
  public ICollection<Ticket>? Tickets { get; set; }
}
