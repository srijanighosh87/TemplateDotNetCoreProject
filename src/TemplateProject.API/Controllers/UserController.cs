using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using TemplateProject.Services.Dtos;
using TemplateProject.Services.Enums;
using TemplateProject.Services.Interfaces;

namespace TemplateProject.API.Controllers;

/// <summary>
/// API to handle user-related requests.
/// </summary>
[ApiVersion(1.0)]
[Route("api/v{v:apiVersion}/users")]
public class UserController : ApiControllerBase
{
  private readonly IUserService _userService;

  /// <summary>
  /// Initializes a new instance of the <see cref="UserController"/> class.
  /// </summary>
  /// <param name="userService">The service for retrieving user information.</param>
  public UserController(IUserService userService)
  {
    _userService = userService;
  }

  /// <summary>
  /// Gets a specific user by ID.
  /// </summary>
  /// <param name="id">The unique identifier of the user to retrieve.</param>
  /// <param name="token">Cancellation token for the request.</param>
  /// <returns>The user details if found; otherwise, a 404 Not Found response.</returns>
  [HttpGet("{id}")]
  [SwaggerResponse(StatusCodes.Status200OK, "Returns the user details.", typeof(UserDto))]
  [SwaggerResponse(StatusCodes.Status404NotFound, "User with the specified ID was not found.", typeof(ProblemDetails))]
  public async Task<ActionResult<UserDto>> GetUser(int id, CancellationToken token)
  {
    var userDto = await _userService.GetUserById(id, token);
    if (userDto == null)
    {
      return NotFound($"User with ID '{id}' not found.");
    }

    return Ok(userDto);
  }

  /// <summary>
  /// Gets a specific user by username.
  /// </summary>
  /// <param name="username">The username of the user to retrieve.</param>
  /// <param name="token">Cancellation token for the request.</param>
  /// <returns>The user details if found; otherwise, a 404 Not Found response.</returns>
  [HttpGet("by-username/{username}")]
  [SwaggerResponse(StatusCodes.Status200OK, "Returns the user details.", typeof(UserDto))]
  [SwaggerResponse(StatusCodes.Status404NotFound, "User with the specified username was not found.", typeof(ProblemDetails))]
  public async Task<ActionResult<UserDto>> GetUserByUsername(string username, CancellationToken token)
  {
    var userDto = await _userService.GetUserByName(username, token);
    if (userDto == null)
    {
      return NotFound($"User with username '{username}' not found.");
    }

    return Ok(userDto);
  }

  /// <summary>
  /// Gets tickets for a user, filtered by ticket status and user IsActive.
  /// </summary>
  /// <param name="id">The unique identifier of the user.</param>
  /// <param name="token">Cancellation token for the request.</param>
  /// <param name="status">Optional ticket status filter.</param>
  /// <param name="isActive">Optional user IsActive filter.</param>
  /// <returns>List of tickets for the user.</returns>
  [HttpGet("{id}/tickets")]
  [SwaggerResponse(StatusCodes.Status200OK, "Returns tickets for the user.", typeof(IEnumerable<TicketDto>))]
  [SwaggerResponse(StatusCodes.Status404NotFound, "User with the specified ID was not found.", typeof(ProblemDetails))]
  public async Task<ActionResult<IEnumerable<TicketDto>>> GetTicketsForUser(int id, CancellationToken token, TicketStatus? status = null, bool? isActive = null)
  {
    var userDto = await _userService.GetUserById(id, token);
    if (userDto == null)
    {
      return NotFound($"User with ID '{id}' not found.");
    }

    var tickets = await _userService.GetTicketsForUser(id, token, status, isActive);
    return Ok(tickets);
  }
}
