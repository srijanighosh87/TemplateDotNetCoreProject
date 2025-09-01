using Microsoft.AspNetCore.Mvc;

namespace TemplateProject.API.Controllers;

/// <summary>
/// Controller base for API endpoints.
/// </summary>
[ApiController]
public abstract class ApiControllerBase : ControllerBase
{
  private static readonly Dictionary<int, (string Title, string DefaultMessage)> s_statusDetails = new()
  {
      { StatusCodes.Status400BadRequest, ("Bad Request", "Request could not be processed.") },
      { StatusCodes.Status401Unauthorized, ("Unauthorized", "Authentication is required and has failed or has not yet been provided.") },
      { StatusCodes.Status403Forbidden, ("Forbidden", "No permission to access this resource.") },
      { StatusCodes.Status404NotFound, ("Not Found", "The requested resource was not found.") },
      { StatusCodes.Status408RequestTimeout, ("Request Timeout", "The server timed out waiting for the request.") },
      { StatusCodes.Status409Conflict, ("Conflict", "The request could not be completed due to a conflict with the current state of the resource.") },
      { StatusCodes.Status412PreconditionFailed, ("Precondition failed.", "Precondition failed.") },
      { StatusCodes.Status415UnsupportedMediaType, ("Unsupported Media Type", "Unsupported media type.") },
      { StatusCodes.Status422UnprocessableEntity, ("Unprocessable entity", "Unprocessable entity.") },
      { StatusCodes.Status499ClientClosedRequest, ("Client closed request.", "Client closed request.") },
      { StatusCodes.Status500InternalServerError, ("Internal Server Error", "An unexpected error occurred while processing your request.") },
      { StatusCodes.Status503ServiceUnavailable, ("Service Unavailable", "The server is currently unable to handle the request due to temporary overloading or maintenance of the server.") },
      { StatusCodes.Status424FailedDependency, ("Failed Dependency", "Failed Dependency") },
      { StatusCodes.Status402PaymentRequired, ("Payment Required", "Payment Required") },
  };

  /// <summary>
  /// Creates an <see cref="ObjectResult"/> that produces a <see cref="ProblemDetails"/> response
  /// with the status code of <see cref="StatusCodes.Status400BadRequest" />.
  /// </summary>
  /// <param name="message">The detailed error message that shall be included in <see cref="ProblemDetails.Detail" />.</param>
  /// <returns>The <see cref="ObjectResult" /> containing the created <see cref="ProblemDetails" /> object.</returns>
  protected ObjectResult BadRequest400(string? message)
  {
    return GetProblemDetails(StatusCodes.Status400BadRequest, message);
  }

  /// <summary>
  /// Creates an <see cref="ObjectResult"/> that produces a <see cref="ProblemDetails"/> response
  /// with the status code of <see cref="StatusCodes.Status401Unauthorized" />.
  /// </summary>
  /// <param name="message">The detailed error message that shall be included in <see cref="ProblemDetails.Detail" />.</param>
  /// <returns>The <see cref="ObjectResult" /> containing the created <see cref="ProblemDetails" /> object.</returns>
  protected ObjectResult Unauthorized401(string? message)
  {
    return GetProblemDetails(StatusCodes.Status401Unauthorized, message);
  }

  /// <summary>
  /// Creates an <see cref="ObjectResult"/> that produces a <see cref="ProblemDetails"/> response
  /// with the status code of <see cref="StatusCodes.Status402PaymentRequired" />.
  /// </summary>
  /// <param name="message">The detailed error message that shall be included in <see cref="ProblemDetails.Detail" />.</param>
  /// <returns>The <see cref="ObjectResult" /> containing the created <see cref="ProblemDetails" /> object.</returns>
  protected ObjectResult PaymentRequired402(string? message)
  {
    return GetProblemDetails(StatusCodes.Status402PaymentRequired, message);
  }

  /// <summary>
  /// Creates an <see cref="ObjectResult"/> that produces a <see cref="ProblemDetails"/> response
  /// with the status code of <see cref="StatusCodes.Status403Forbidden" />.
  /// </summary>
  /// <param name="message">The detailed error message that shall be included in <see cref="ProblemDetails.Detail" />.</param>
  /// <returns>The <see cref="ObjectResult" /> containing the created <see cref="ProblemDetails" /> object.</returns>
  protected ObjectResult Forbidden403(string? message)
  {
    return GetProblemDetails(StatusCodes.Status403Forbidden, message);
  }

  /// <summary>
  /// Creates an <see cref="ObjectResult"/> that produces a <see cref="ProblemDetails"/> response
  /// with the status code of <see cref="StatusCodes.Status404NotFound" />.
  /// </summary>
  /// <param name="message">The detailed error message that shall be included in <see cref="ProblemDetails.Detail" />.</param>
  /// <returns>The <see cref="ObjectResult" /> containing the created <see cref="ProblemDetails" /> object.</returns>
  protected ObjectResult NotFound404(string? message = null)
  {
    return GetProblemDetails(StatusCodes.Status404NotFound, message);
  }

  /// <summary>
  /// Creates an <see cref="ObjectResult"/> that produces a <see cref="ProblemDetails"/> response
  /// with the status code of <see cref="StatusCodes.Status408RequestTimeout" />.
  /// </summary>
  /// <param name="message">The detailed error message that shall be included in <see cref="ProblemDetails.Detail" />.</param>
  /// <returns>The <see cref="ObjectResult" /> containing the created <see cref="ProblemDetails" /> object.</returns>
  protected ObjectResult RequestTimeout408(string? message)
  {
    return GetProblemDetails(StatusCodes.Status408RequestTimeout, message);
  }

  /// <summary>
  /// Creates an <see cref="ObjectResult"/> that produces a <see cref="ProblemDetails"/> response
  /// with the status code of <see cref="StatusCodes.Status409Conflict" />.
  /// </summary>
  /// <param name="message">The detailed error message that shall be included in <see cref="ProblemDetails.Detail" />.</param>
  /// <returns>The <see cref="ObjectResult" /> containing the created <see cref="ProblemDetails" /> object.</returns>
  protected ObjectResult Conflict409(string? message)
  {
    return GetProblemDetails(StatusCodes.Status409Conflict, message);
  }

  /// <summary>
  /// Creates an <see cref="ObjectResult"/> that produces a <see cref="ProblemDetails"/> response
  /// with the status code of <see cref="StatusCodes.Status412PreconditionFailed" />.
  /// </summary>
  /// <param name="message">The detailed error message that shall be included in <see cref="ProblemDetails.Detail" />.</param>
  /// <returns>The <see cref="ObjectResult" /> containing the created <see cref="ProblemDetails" /> object.</returns>
  protected ObjectResult PreconditionFailed412(string? message)
  {
    return GetProblemDetails(StatusCodes.Status412PreconditionFailed, message);
  }

  /// <summary>
  /// Creates an <see cref="ObjectResult"/> that produces a <see cref="ProblemDetails"/> response
  /// with the status code of <see cref="StatusCodes.Status415UnsupportedMediaType" />.
  /// </summary>
  /// <param name="message">The detailed error message that shall be included in <see cref="ProblemDetails.Detail" />.</param>
  /// <returns>The <see cref="ObjectResult" /> containing the created <see cref="ProblemDetails" /> object.</returns>
  protected ObjectResult UnsupportedMediaType415(string? message)
  {
    return GetProblemDetails(StatusCodes.Status415UnsupportedMediaType, message);
  }

  /// <summary>
  /// Creates an <see cref="ObjectResult"/> that produces a <see cref="ProblemDetails"/> response
  /// with the status code of <see cref="StatusCodes.Status422UnprocessableEntity" />.
  /// </summary>
  /// <param name="message">The detailed error message that shall be included in <see cref="ProblemDetails.Detail" />.</param>
  /// <returns>The <see cref="ObjectResult" /> containing the created <see cref="ProblemDetails" /> object.</returns>
  protected ObjectResult UnprocessableEntity422(string? message)
  {
    return GetProblemDetails(StatusCodes.Status422UnprocessableEntity, message);
  }

  /// <summary>
  /// Creates an <see cref="ObjectResult"/> that produces a <see cref="ProblemDetails"/> response
  /// with the status code of <see cref="StatusCodes.Status424FailedDependency" />.
  /// </summary>
  /// <param name="message">The detailed error message that shall be included in <see cref="ProblemDetails.Detail" />.</param>
  /// <returns>The <see cref="ObjectResult" /> containing the created <see cref="ProblemDetails" /> object.</returns>
  protected ObjectResult FailedDependency424(string? message)
  {
    return GetProblemDetails(StatusCodes.Status424FailedDependency, message);
  }

  /// <summary>
  /// Creates an <see cref="ObjectResult"/> that produces a <see cref="ProblemDetails"/> response
  /// with the status code of <see cref="StatusCodes.Status499ClientClosedRequest" />.
  /// </summary>
  /// <param name="message">The detailed error message that shall be included in <see cref="ProblemDetails.Detail" />.</param>
  /// <returns>The <see cref="ObjectResult" /> containing the created <see cref="ProblemDetails" /> object.</returns>
  protected ObjectResult ClientClosedRequest499(string? message)
  {
    return GetProblemDetails(StatusCodes.Status499ClientClosedRequest, message);
  }

  /// <summary>
  /// Creates an <see cref="ObjectResult"/> that produces a <see cref="ProblemDetails"/> response
  /// with the status code of <see cref="StatusCodes.Status500InternalServerError" />.
  /// </summary>
  /// <param name="message">The detailed error message that shall be included in <see cref="ProblemDetails.Detail" />.</param>
  /// <returns>The <see cref="ObjectResult" /> containing the created <see cref="ProblemDetails" /> object.</returns>
  protected ObjectResult Error500(string? message)
  {
    return GetProblemDetails(StatusCodes.Status500InternalServerError, message);
  }

  /// <summary>
  /// Creates an <see cref="ObjectResult"/> that produces a <see cref="ProblemDetails"/> response
  /// with the status code of <see cref="StatusCodes.Status503ServiceUnavailable" />.
  /// </summary>
  /// <param name="message">The detailed error message that shall be included in <see cref="ProblemDetails.Detail" />.</param>
  /// <returns>The <see cref="ObjectResult" /> containing the created <see cref="ProblemDetails" /> object.</returns>
  protected ObjectResult ServiceUnavailable503(string? message)
  {
    return GetProblemDetails(StatusCodes.Status503ServiceUnavailable, message);
  }

  /// <summary>
  /// Map status codes to corresponding methods that creates an <see cref="ObjectResult"/> response
  /// with the status code of <paramref name="statusCode"/>.
  /// </summary>
  /// <param name="statusCode">HTTP status codes.</param>
  /// <param name="message">The detailed error message that shall be included in <see cref="ProblemDetails.Detail" />.</param>
  /// <returns>The <see cref="ObjectResult" /> containing the created <see cref="ProblemDetails" /> object.</returns>
  protected ObjectResult GetProblemDetails(int statusCode, string? message)
  {
    return s_statusDetails.TryGetValue(statusCode, out var value)
       ? Problem(detail: message ?? value.DefaultMessage, statusCode: statusCode, title: value.Title)
       : Problem(detail: message ?? "Unknown status code", statusCode: statusCode, title: "Error");
  }
}
