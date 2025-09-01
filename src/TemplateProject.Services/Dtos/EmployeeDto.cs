using Riok.Mapperly.Abstractions;
using TemplateProject.Services.Database.Models;

namespace TemplateProject.Services.Dtos;

/// <summary>
/// Data transfer object for Employee entity.
/// </summary>
public class EmployeeDto
{
  /// <summary>
  /// Unique identifier for the employee.
  /// </summary>
  public int EmployeeId { get; set; }

  /// <summary>
  /// Name of the employee.
  /// </summary>
  public string Name { get; set; } = string.Empty;

  /// <summary>
  /// Email address of the employee.
  /// </summary>
  public string Email { get; set; } = string.Empty;

  /// <summary>
  /// List of ticket IDs assigned to the employee.
  /// </summary>
  public List<string>? TicketIds { get; set; }
}

/// <summary>
/// Provides mapping functionality between <see cref="Employee"/> and <see cref="EmployeeDto"/> objects.
/// </summary>
[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public static partial class EmployeeMapper
{
  /// <summary>
  /// Converts an <see cref="Employee"/> object to an <see cref="EmployeeDto"/> object.
  /// </summary>
  /// <param name="employee">The <see cref="Employee"/> instance to convert. Cannot be <see langword="null"/>.</param>
  [MapProperty(nameof(employee.Tickets), nameof(EmployeeDto.TicketIds), Use = nameof(MapTicketsToTicketIds))]
  public static partial EmployeeDto ToDto(this Employee employee);

  private static List<int>? MapTicketsToTicketIds(ICollection<Ticket>? tickets)
  {
    return tickets?.Select(t => t.TicketId).ToList();
  }
}
