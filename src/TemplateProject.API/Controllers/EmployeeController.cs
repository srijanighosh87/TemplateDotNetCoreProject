using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using TemplateProject.Services.Dtos;
using TemplateProject.Services.Interfaces;

namespace TemplateProject.API.Controllers;

/// <summary>
/// API to handle employee-related requests.
/// </summary>
[ApiVersion(1.0)]
[Route("api/v{v:apiVersion}/employees")]
public class EmployeeController : ApiControllerBase
{
  private readonly IEmployeeService _employeeService;

  /// <summary>
  /// Initializes a new instance of the <see cref="EmployeeController"/> class.
  /// </summary>
  /// <param name="employeeService">The service for retrieving employee information.</param>
  public EmployeeController(IEmployeeService employeeService)
  {
    _employeeService = employeeService;
  }

  /// <summary>
  /// Gets a specific employee by ID.
  /// </summary>
  /// <param name="id">The unique identifier of the employee to retrieve.</param>
  /// <param name="token">The cancellation token.</param>
  /// <returns>The employee details if found.</returns>
  [HttpGet("{id}")]
  [SwaggerResponse(StatusCodes.Status200OK, "Returns the employee details.", typeof(EmployeeDto))]
  [SwaggerResponse(StatusCodes.Status404NotFound, "Employee with the specified ID was not found.", typeof(ProblemDetails))]
  public async Task<ActionResult<EmployeeDto>> GetEmployee(int id, CancellationToken token)
  {
    var employeeDto = await _employeeService.GetEmployeeById(id, token);
    if (employeeDto == null)
    {
      return NotFound($"Employee with ID '{id}' not found.");
    }

    return Ok(employeeDto);
  }

  /// <summary>
  /// Gets all employees.
  /// </summary>
  /// <param name="token">The cancellation token.</param>
  /// <returns>List of all employees.</returns>
  [HttpGet]
  [SwaggerResponse(StatusCodes.Status200OK, "Returns all employees.", typeof(IEnumerable<EmployeeDto>))]
  public async Task<ActionResult<IEnumerable<EmployeeDto>>> GetAllEmployees(CancellationToken token)
  {
    var employees = await _employeeService.GetAllEmployees(token);
    return Ok(employees);
  }
}
