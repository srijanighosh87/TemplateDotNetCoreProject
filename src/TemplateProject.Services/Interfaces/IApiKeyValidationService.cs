namespace TemplateProject.Services.Interfaces;

/// <summary>
/// Handles API key validation logic.
/// </summary>
public interface IApiKeyValidationService
{
  /// <summary>
  /// Checks if given <paramref name="clientApiKey" /> is valid and still in use.
  /// </summary>
  /// <param name="clientApiKey">The client's API key.</param>
  /// <returns><see langword="true" /> if the <paramref name="clientApiKey" /> is valid and still in use; otherwise <see langword="false" />.</returns>
  bool IsValidApiKey(string clientApiKey);
}
