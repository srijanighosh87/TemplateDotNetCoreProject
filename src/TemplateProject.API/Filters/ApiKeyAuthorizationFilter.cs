using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using TemplateProject.Services.Interfaces;

namespace TemplateProject.API.Filters;

/// <summary>
/// An authorization filter that validates API key headers in incoming HTTP requests.
/// </summary>
public class ApiKeyAuthorizationFilter : IAuthorizationFilter
{
  private const string ApiKeyHeaderName = "X-API-Key";

  private readonly IApiKeyValidationService _apiKeyValidationService;
  private readonly ProblemDetailsFactory _problemDetailsFactory;

  /// <summary>
  /// Initializes a new instance of the <see cref="ApiKeyAuthorizationFilter" /> class.
  /// </summary>
  /// <param name="apiKeyValidationService">Injected <see cref="IApiKeyValidationService" /> instance.</param>
  /// <param name="problemDetailsFactory">Injected <see cref="ProblemDetailsFactory" /> instance.</param>
  public ApiKeyAuthorizationFilter(
    IApiKeyValidationService apiKeyValidationService,
    ProblemDetailsFactory problemDetailsFactory)
  {
    _apiKeyValidationService = apiKeyValidationService;
    _problemDetailsFactory = problemDetailsFactory;
  }

  /// <inheritdoc />
  public void OnAuthorization(AuthorizationFilterContext context)
  {
    var httpContext = context.HttpContext;
    string? apiKey = httpContext.Request.Headers[ApiKeyHeaderName];

    if (string.IsNullOrEmpty(apiKey))
    {
      context.Result = CreateProblemResult(StatusCodes.Status401Unauthorized, "Missing API key");
    }
    else if (!_apiKeyValidationService.IsValidApiKey(apiKey))
    {
      context.Result = CreateProblemResult(StatusCodes.Status401Unauthorized, "Invalid API key");
    }

    ObjectResult CreateProblemResult(int statusCode, string title)
    {
      var problemDetails = _problemDetailsFactory.CreateProblemDetails(httpContext, statusCode, title);

      return new ObjectResult(problemDetails)
      {
        StatusCode = problemDetails.Status,
      };
    }
  }
}
