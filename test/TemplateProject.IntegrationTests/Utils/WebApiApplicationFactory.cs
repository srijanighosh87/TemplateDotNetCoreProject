//using System.Net.Http.Json;
//using System.Text.Json;
//using System.Text.Json.Serialization;
//using Microsoft.AspNetCore.Hosting;
//using Microsoft.AspNetCore.Mvc.Testing;
//using Microsoft.Extensions.DependencyInjection;
//using Microsoft.Extensions.Hosting;
//using Microsoft.Extensions.Logging

//namespace TemplateProject.IntegrationTests.Utils;

///// <summary>
///// Custom <see cref="WebApplicationFactory{T}" /> for the <see cref="WebApiFixture" />.
///// </summary>
//public sealed class WebApiApplicationFactory : WebApplicationFactory<Program>
//{
//  private const string DefaultConnectionString = "Host=localhost;Port=5432;Username=postgres;Password=postgres";
//  private const string FallbackConnectionString = "Host=localhost;Port=5432;Username=postgres;Password=7csx14a7";
//  private const string IntegrationTestDbName = "integration_test_db";
//  private const string ApiKeyHeaderName = "X-API-Key";
//  private const string ValidApiKey = "ABCXYZ";
//  private const string BaseUrl = "http://localhost:7118/api/v1.0/";

//  /// <summary>
//  /// Initializes a new instance of the <see cref="WebApiApplicationFactory" /> class.
//  /// </summary>
//  public WebApiApplicationFactory()
//  {
//    ClientOptions.BaseAddress = new Uri(BaseUrl);
//  }

//  /// <summary>
//  /// Gets the <see cref="JsonSerializerOptions"/> used for JSON serialization and deserialization.
//  /// </summary>
//  public JsonSerializerOptions JsonOptions { get; } = new JsonSerializerOptions(JsonSerializerDefaults.Web)
//  {
//    Converters = { new JsonStringEnumConverter() },
//  };

//  /// <summary>
//  /// Creates an HttpClient with the API key header already set.
//  /// </summary>
//  /// <returns>An HttpClient configured with the API key header.</returns>
//  public HttpClient CreateClientWithApiKey()
//  {
//    var client = CreateClient();
//    client.DefaultRequestHeaders.Add(ApiKeyHeaderName, ValidApiKey);
//    return client;
//  }

//  /// <summary>
//  /// Reads and deserializes the JSON content from the specified HTTP response into an object of type <typeparamref name="T"/>.
//  /// </summary>
//  /// <typeparam name="T">The type of the object to deserialize the JSON content into.</typeparam>
//  /// <param name="response">The <see cref="HttpResponseMessage"/> containing the JSON content to read.</param>
//  /// <param name="cancellationToken">A <see cref="CancellationToken"/> that can be used to cancel the operation.</param>
//  public async Task<T?> ReadFromJsonAsync<T>(HttpResponseMessage response, CancellationToken cancellationToken = default)
//  {
//    return await response.Content.ReadFromJsonAsync<T>(JsonOptions, cancellationToken);
//  }

//  /// <inheritdoc/>
//  protected override void ConfigureWebHost(IWebHostBuilder builder)
//  {
//    string finalConnString;

//    using var dbContext = new DatabaseContext(
//      new DbContextOptionsBuilder<DatabaseContext>()
//        .UseNpgsql(DefaultConnectionString)
//        .Options);
//    if (dbContext.Database.CanConnect())
//    {
//      finalConnString = $"{DefaultConnectionString};Database={IntegrationTestDbName}";
//    }
//    else
//    {
//      finalConnString = $"{FallbackConnectionString};Database={IntegrationTestDbName}";
//    }

//    builder.UseSetting("ConnectionStrings:DefaultConnection", finalConnString)
//      .ConfigureServices(services =>
//      {
//        services.AddSingleton<DatabaseCleanupService>();
//        services.AddHostedService(sp => sp.GetRequiredService<DatabaseCleanupService>());
//      });
//  }

//  /// <summary>
//  /// Creates the host for the application, ensuring that migrations are applied before seeding the database.
//  /// </summary>
//  /// <param name="builder">Host builder.</param>
//  protected override IHost CreateHost(IHostBuilder builder)
//  {
//    var host = base.CreateHost(builder);
//    using var db = host.Services.GetRequiredService<IDbContextFactory<DatabaseContext>>().CreateDbContext();
//    if (!db.Conversions.Any())
//    {
//      SeedDatabase(db);
//    }

//    return host;
//  }

//  /// <summary>
//  /// Seeds the database with initial data for testing purposes.
//  /// </summary>
//  /// <param name="dbContext">database context.</param>
//  private static void SeedDatabase(DatabaseContext dbContext)
//  {
//    dbContext.AddRange(
//      new Conversion
//      {
//        CustomerId = "TEST_CUSTOMER_001",
//        VoyageId = "TEST_VOYAGE_001",
//        VoyagePlanSchemaVersion = "1.0.0",
//        ShipName = "TEST_SHIP_1",
//        Template = [0x01, 0x02, 0x03],
//        VoyagePlan = "Test data for voyage plan json.",
//        TimeCreated = DateTime.UtcNow,
//        TimeStarted = DateTime.UtcNow,
//        TimeFinished = DateTime.UtcNow,
//        Status = ConversionStatus.Processing,
//        Message = "Test conversion is processing.",
//        Xlsx = null,
//        Logs =
//        [
//          new ConversionLog
//          {
//            TimeCreated = DateTime.UtcNow,
//            Level = LogLevel.Information,
//            Message = "Starting conversion process",
//          },
//          new ConversionLog
//          {
//            TimeCreated = DateTime.UtcNow,
//            Level = LogLevel.Debug,
//            Message = "Loading template file",
//          },
//          new ConversionLog
//          {
//            TimeCreated = DateTime.UtcNow,
//            Level = LogLevel.Warning,
//            Message = "Some waypoints have incomplete data",
//          }
//        ],
//      },
//      new Conversion
//      {
//        CustomerId = "TEST_CUSTOMER_002",
//        VoyageId = "TEST_VOYAGE_002",
//        VoyagePlanSchemaVersion = "1.0.0",
//        ShipName = "TEST_SHIP_2",
//        Template = [0x01, 0x02, 0x03],
//        VoyagePlan = "Test data for voyage plan json.",
//        TimeCreated = DateTime.UtcNow,
//        TimeStarted = DateTime.UtcNow,
//        TimeFinished = DateTime.UtcNow,
//        Status = ConversionStatus.Success,
//        Message = "Test conversion succeeded",
//        Xlsx = [0x07, 0x08, 0x09],
//        Logs =
//        [
//          new ConversionLog
//          {
//            TimeCreated = DateTime.UtcNow,
//            Level = LogLevel.Information,
//            Message = "Starting conversion process",
//          },
//          new ConversionLog
//          {
//            TimeCreated = DateTime.UtcNow,
//            Level = LogLevel.Debug,
//            Message = "Template loaded successfully",
//          },
//          new ConversionLog
//          {
//            TimeCreated = DateTime.UtcNow,
//            Level = LogLevel.Information,
//            Message = "Converting voyage plan data",
//          },
//          new ConversionLog
//          {
//            TimeCreated = DateTime.UtcNow,
//            Level = LogLevel.Information,
//            Message = "Conversion completed successfully",
//          }
//        ],
//      },
//      new Conversion
//      {
//        CustomerId = "TEST_CUSTOMER_003",
//        VoyageId = "TEST_VOYAGE_003",
//        VoyagePlanSchemaVersion = "2.0.0",
//        ShipName = "TEST_SHIP_3",
//        Template = [0x01, 0x02, 0x03],
//        VoyagePlan = "Test data for voyage plan json.",
//        TimeCreated = DateTime.UtcNow,
//        TimeStarted = DateTime.UtcNow,
//        TimeFinished = DateTime.UtcNow,
//        Status = ConversionStatus.Success,
//        Message = "Test conversion Completed Successfully",
//        Xlsx = [0x07, 0x08, 0x09],
//      },
//      new Conversion
//      {
//        CustomerId = "TEST_CUSTOMER_001",
//        VoyageId = "TEST_VOYAGE_004",
//        VoyagePlanSchemaVersion = "2.0.0",
//        ShipName = "TEST_SHIP_1",
//        Template = [0x01, 0x02, 0x03],
//        VoyagePlan = "Test data for voyage plan json.",
//        TimeCreated = DateTime.UtcNow,
//        TimeStarted = DateTime.UtcNow,
//        TimeFinished = DateTime.UtcNow,
//        Status = ConversionStatus.Failure,
//        Message = "Test conversion failed",
//        Xlsx = null,
//      },
//      new Conversion
//      {
//        CustomerId = "TEST_CUSTOMER_002",
//        VoyageId = "TEST_VOYAGE_005",
//        VoyagePlanSchemaVersion = "1.0.0",
//        ShipName = "TEST_SHIP_2",
//        Template = [0x01, 0x02, 0x03],
//        VoyagePlan = "Test data for voyage plan json.",
//        TimeCreated = DateTime.UtcNow.Date,
//        TimeStarted = DateTime.UtcNow,
//        TimeFinished = DateTime.UtcNow,
//        Status = ConversionStatus.Processing,
//        Message = "Test conversion is processing.",
//        Xlsx = null,
//      },
//      new Conversion
//      {
//        CustomerId = "TEST_CUSTOMER_003",
//        VoyageId = "TEST_VOYAGE_006",
//        VoyagePlanSchemaVersion = "2.0.0",
//        ShipName = "TEST_SHIP_4",
//        Template = [0x01, 0x02, 0x03],
//        VoyagePlan = "Test data for voyage plan json.",
//        TimeCreated = DateTime.UtcNow,
//        TimeStarted = DateTime.UtcNow,
//        TimeFinished = DateTime.UtcNow,
//        Status = ConversionStatus.Success,
//        Message = "Test conversion Completed Successfully",
//        Xlsx = [0x07, 0x08, 0x09],
//        Logs = Array.Empty<ConversionLog>(),
//      });
//    dbContext.SaveChanges();
//  }

//  /// <inheritdoc/>
//  public override async ValueTask DisposeAsync()
//  {
//    await using var cleanupContext = ObtainTestDbContext();
//    try
//    {
//      await cleanupContext.Database.EnsureDeletedAsync();
//      await cleanupContext.DisposeAsync();
//    }
//    catch (Exception ex)
//    {
//      var logger = Services.GetRequiredService<ILogger<WebApiApplicationFactory>>();
//      logger.LogError(ex, "Error during async test cleanup.");
//    }

//    await base.DisposeAsync();
//  }

//  /// <summary>
//  /// Obtains a new instance of <see cref="DatabaseContext"/> for testing purposes.
//  /// </summary>
//  public DatabaseContext ObtainTestDbContext()
//  {
//    return Services.GetRequiredService<IDbContextFactory<DatabaseContext>>().CreateDbContext();
//  }
//}
