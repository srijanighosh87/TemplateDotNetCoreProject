using System.Reflection;
using Asp.Versioning;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;
using TemplateProject.API.Utils;

namespace TemplateProject.API.Extensions;

/// <summary>
/// Extension methods for configuring application services.
/// </summary>
public static class ServiceConfiguration
{
  /// <summary>
  /// Configures Swagger/OpenAPI documentation for the API.
  /// </summary>
  /// <param name="services">The service collection to configure.</param>
  /// <returns>The service collection for method chaining.</returns>
  public static IServiceCollection ConfigureSwagger(this IServiceCollection services)
  {
    services.AddSwaggerGen(options =>
    {
      // Parse the XML comments file for API documentation
      string xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
      string xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFilename);
      options.IncludeXmlComments(xmlPath);
      options.DocumentFilter<TagDescriptionsDocumentFilter>(xmlPath);
      options.EnableAnnotations();
    });
    services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
    return services;
  }

  /// <summary>
  /// Registers API versioning services.
  /// </summary>
  /// <param name="services">The service collection to configure.</param>
  /// <param name="defaultApiVersion">Api version.</param>
  public static IServiceCollection AddApiVersioningServices(this IServiceCollection services, ApiVersion defaultApiVersion)
  {
    services.AddApiVersioning(options =>
    {
      options.DefaultApiVersion = defaultApiVersion;
      options.AssumeDefaultVersionWhenUnspecified = true;
      options.ReportApiVersions = true;
    })
    .AddApiExplorer(options =>
    {
      options.GroupNameFormat = "'v'VVV";
      options.SubstituteApiVersionInUrl = true;
    });

    return services;
  }
}
