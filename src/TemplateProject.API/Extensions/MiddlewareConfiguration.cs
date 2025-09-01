using Asp.Versioning.ApiExplorer;
using Scalar.AspNetCore;

namespace TemplateProject.API.Extensions;

/// <summary>
/// Provides configuration methods for setting up middleware in a web application.
/// </summary>
public static class MiddlewareConfiguration
{
  /// <summary>
  /// Configures the Scalar API reference endpoints for the application.
  /// </summary>
  /// <param name="app">The <see cref="WebApplication"/> instance to configure. This must have access to an <see
  /// cref="IApiVersionDescriptionProvider"/> service to retrieve API version information.</param>
  public static void ConfigureScalarApi(this WebApplication app)
  {
    var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
    app.MapScalarApiReference("/api-reference", options =>
    {
      foreach (var description in provider.ApiVersionDescriptions)
      {
        var group = description.GroupName;
        options.AddDocument(group, group);
      }
    });
  }
}
