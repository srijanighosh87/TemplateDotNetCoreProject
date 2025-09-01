using System.Configuration;
using Microsoft.Extensions.Configuration;
using TemplateProject.Services.Interfaces;

namespace TemplateProject.Services.Services;

internal class ApiKeyValidationService : IApiKeyValidationService
{
  private const string ApiKeyConfigName = "ApiKey";

  private readonly IConfiguration _configuration;

  public ApiKeyValidationService(IConfiguration configuration)
  {
    _configuration = configuration;
  }

  public bool IsValidApiKey(string clientApiKey)
  {
    if (string.IsNullOrEmpty(clientApiKey))
    {
      return false;
    }

    string apiKey = _configuration[ApiKeyConfigName] ?? throw new ConfigurationErrorsException("API key is not configured.");
    return apiKey == clientApiKey;
  }
}
