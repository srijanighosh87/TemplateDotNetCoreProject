using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using TemplateProject.Services.Dtos;
using TemplateProject.Services.Interfaces;

namespace TemplateProject.API.Controllers;

/// <summary>
/// API to handle ticket-related requests.
/// </summary>
[ApiVersion(1.0)]
[Route("api/v{v:apiVersion}/tickets")]
public class TicketController : ApiControllerBase
{
  private readonly ITicketService _ticketService;

  /// <summary>
  /// Initializes a new instance of the <see cref="TicketController"/> class.
  /// </summary>
  /// <param name="ticketService">The service for retrieving ticket information.</param>
  public TicketController(ITicketService ticketService)
  {
    _ticketService = ticketService;
  }

  /// <summary>
  /// Gets a specific ticket by ID.
  /// </summary>
  /// <param name="id">The unique identifier of the ticket to retrieve.</param>
  /// <param name="token">Cancellation token for the request.</param>
  /// <returns>The ticket details if found; otherwise, a 404 Not Found response.</returns>
  [HttpGet("{id}")]
  [SwaggerResponse(StatusCodes.Status200OK, "Returns the ticket details.", typeof(TicketDto))]
  [SwaggerResponse(StatusCodes.Status404NotFound, "Ticket with the specified ID was not found.", typeof(ProblemDetails))]
  public async Task<ActionResult<TicketDto>> GetTicket(int id, CancellationToken token)
  {
    var ticketDto = await _ticketService.GetTicketById(id, token);
    if (ticketDto == null)
    {
      return NotFound($"Ticket with ID '{id}' not found.");
    }

    return Ok(ticketDto);
  }

  /// <summary>
  /// Gets tickets filtered by user, date, or assigned employee.
  /// </summary>
  /// <param name="userId">Optional user id who created the ticket.</param>
  /// <param name="createdBefore">Optional filter for tickets created before this date.</param>
  /// <param name="createdAfter">Optional filter for tickets created after this date.</param>
  /// <param name="assignedToEmployeeId">Optional employee id to whom the ticket is assigned.</param>
  /// <param name="resultCount">Optional number of results to return. Default value is 20.</param>
  /// <param name="token">Cancellation token for the request.</param>
  /// <returns>List of filtered tickets.</returns>
  [HttpGet]
  [SwaggerResponse(StatusCodes.Status200OK, "Returns filtered tickets.", typeof(IEnumerable<TicketDto>))]
  public async Task<ActionResult<IEnumerable<TicketDto>>> GetTickets(
      [FromQuery] int? userId = null,
      [FromQuery] DateTime? createdBefore = null,
      [FromQuery] DateTime? createdAfter = null,
      [FromQuery] int? assignedToEmployeeId = null,
      [FromQuery] int? resultCount = null,
      CancellationToken token = default)
  {
    var tickets = await _ticketService.GetTickets(userId, createdBefore, createdAfter, assignedToEmployeeId, resultCount, token);
    return Ok(tickets);
  }

  /// <summary>
  /// Creates a new ticket for a user.
  /// </summary>
  /// <param name="userId">The unique identifier of the user who creates the ticket.</param>
  /// <param name="subject">The subject of the ticket.</param>
  /// <param name="description">The description of the ticket.</param>
  /// <param name="token">Cancellation token for the request.</param>
  /// <returns>The created ticket ID if successful; otherwise, a 400 Bad Request response.</returns>
  [HttpPost]
  [SwaggerResponse(StatusCodes.Status201Created, "Returns the created ticket ID.", typeof(string))]
  [SwaggerResponse(StatusCodes.Status400BadRequest, "User not found or invalid input.", typeof(ProblemDetails))]
  public async Task<ActionResult<string>> CreateTicket(int userId, string subject, string description, CancellationToken token)
  {
    if (userId == 0 || string.IsNullOrWhiteSpace(subject) || string.IsNullOrWhiteSpace(description))
    {
      return BadRequest("UserId, Subject, and Description are required.");
    }

    var ticketId = await _ticketService.CreateTicket(userId, subject, description, token);
    if (ticketId == null)
    {
      return BadRequest($"User with ID '{userId}' not found. Ticket not created.");
    }

    return CreatedAtAction(nameof(GetTicket), new { id = ticketId }, ticketId);
  }
}
