using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TemplateProject.Services.Dtos;
using TemplateProject.Services.Interfaces;
using TemplateProject.Services.Database;

namespace TemplateProject.Services.Services;

/// <summary>
/// Implementation of <see cref="IEmployeeService"/> for managing and retrieving employee information.
/// </summary>
public class EmployeeService : IEmployeeService
{
  private readonly IDbContextFactory<DatabaseContext> _dbContextFactory;
  private readonly ILogger<EmployeeService> _logger;

  /// <summary>
  /// Initializes a new instance of the <see cref="EmployeeService"/> class.
  /// </summary>
  /// <param name="dbContextFactory">A factory for creating instances of <see cref="DatabaseContext"/>. This is used to interact with the database.</param>
  /// <param name="logger">An instance of <see cref="ILogger{TCategoryName}"/></param>
  public EmployeeService(IDbContextFactory<DatabaseContext> dbContextFactory, ILogger<EmployeeService> logger)
  {
    _dbContextFactory = dbContextFactory;
    _logger = logger;
  }

  /// <inheritdoc />
  public async Task<EmployeeDto?> GetEmployeeById(int employeeId, CancellationToken cancellationToken)
  {
    _logger.LogInformation("Retrieving employee with ID: {EmployeeId}", employeeId);
    await using var dbContext = _dbContextFactory.CreateDbContext();
    var employee = await dbContext.Employees
        .Include(e => e.Tickets)
        .FirstOrDefaultAsync(e => e.EmployeeId == employeeId, cancellationToken);
    return employee?.ToDto();
  }

  /// <inheritdoc />
  public async Task<IEnumerable<EmployeeDto>> GetAllEmployees(CancellationToken cancellationToken)
  {
    _logger.LogInformation("Retrieving all employees from the database.");
    await using var dbContext = _dbContextFactory.CreateDbContext();
    var employees = await dbContext.Employees
        .Include(e => e.Tickets)
        .ToListAsync(cancellationToken);
    return employees.Select(e => e.ToDto());
  }
}
