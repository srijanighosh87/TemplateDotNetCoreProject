using TemplateProject.Services.Dtos;

namespace TemplateProject.Services.Interfaces;

/// <summary>
/// Interface for managing and retrieving employee information.
/// </summary>
public interface IEmployeeService
{
  /// <summary>
  /// Gets an employee by their unique identifier.
  /// </summary>
  /// <param name="employeeId">The unique identifier of the employee.</param>
  /// <param name="cancellationToken">Optional cancellation token.</param>
  Task<EmployeeDto?> GetEmployeeById(int employeeId, CancellationToken cancellationToken);

  /// <summary>
  /// Gets all employees.
  /// </summary>
  /// <param name="cancellationToken">Optional cancellation token.</param>
  Task<IEnumerable<EmployeeDto>> GetAllEmployees(CancellationToken cancellationToken);
}
