using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TDM.Api.Client.Common;
using Refit;

namespace TDM.Api.Client.Extensions;

/// <summary>
/// Extension methods to register Refit API clients for the ToDoMaui API.
/// </summary>
public static class RefitExtensions
{
    /// <summary>
    /// Registers all Refit API clients and configures their base address.
    /// Uses IApiClientMarker marker interface to identify clients.
    /// </summary>
    /// <param name="services">Service collection.</param>
    /// <param name="baseUrl">Base URL of the ToDoMaui API.</param>
    /// <returns>The updated service collection.</returns>
    public static IServiceCollection ConfigureRefit(this IServiceCollection services, Uri baseUrl)
    {
        Assembly assembly = typeof(RefitExtensions).Assembly;

        var apiClientInterfaces = assembly.GetTypes()
            .Where(type => type.IsInterface &&
                           type != typeof(IApiClientMarker) &&
                           typeof(IApiClientMarker).IsAssignableFrom(type))
            .ToList();

        // Настройки JSON сериализации для Refit
        var refitSettings = new RefitSettings
        {
            ContentSerializer = new SystemTextJsonContentSerializer(new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                Converters =
                {
                    new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) // Сериализуем enum как строки
                }
            })
        };

        foreach (Type clientInterface in apiClientInterfaces)
        {
            services
                .AddRefitClient(clientInterface, refitSettings) // Передаём настройки
                .ConfigureHttpClient(c =>
                {
                    c.BaseAddress = baseUrl;
                    c.Timeout = TimeSpan.FromSeconds(30); // Увеличиваем timeout для мобильных устройств
                })
#if DEBUG
                .AddHttpMessageHandler(sp =>
                {
                    var logger = sp.GetRequiredService<ILogger<HttpLoggingHandler>>();
                    return new HttpLoggingHandler(logger);
                })
#endif
                ;
        }

        return services;
    }
}

#if DEBUG
/// <summary>
/// HTTP message handler for logging requests and responses in debug mode.
/// </summary>
internal class HttpLoggingHandler : DelegatingHandler
{
    private readonly ILogger<HttpLoggingHandler> _logger;

    public HttpLoggingHandler(ILogger<HttpLoggingHandler> logger)
    {
        _logger = logger;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("HTTP Request: {Method} {Uri}", request.Method, request.RequestUri);

        try
        {
            var response = await base.SendAsync(request, cancellationToken);
            _logger.LogInformation("HTTP Response: {StatusCode} from {Uri}", (int)response.StatusCode, request.RequestUri);
            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "HTTP Request failed: {Method} {Uri}", request.Method, request.RequestUri);
            throw;
        }
    }
}
#endif
