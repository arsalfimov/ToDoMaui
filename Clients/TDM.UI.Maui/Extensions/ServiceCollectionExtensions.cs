using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TDM.Api.Client.Extensions;
using TDM.UI.Maui.Options;
using TDM.UI.Maui.Services;

namespace TDM.UI.Maui.Extensions;

/// <summary>
/// Extension methods for configuring application services.
/// </summary>
public static class ServiceCollectionExtensions
{
    private const string Server = "Server";

    /// <summary>
    /// Registers core application services including API client configuration.
    /// Validates configuration settings at startup.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="configuration">Application configuration containing Server section.</param>
    /// <returns>The updated service collection.</returns>
    public static IServiceCollection AddCoreServices(this IServiceCollection services, IConfiguration configuration)
    {
        var serverOptions = configuration.GetSection(Server).Get<ServerOption>() 
                            ?? throw new InvalidOperationException("Server:ApiUrl configuration is missing");

        string apiUrl = serverOptions.GetApiUrl();

        // Логируем используемый URL для отладки
        Console.WriteLine($"[API Configuration] Using API URL: {apiUrl}");
#if ANDROID
        Console.WriteLine("[API Configuration] Platform: Android");
#elif IOS
        Console.WriteLine("[API Configuration] Platform: iOS");
#else
        Console.WriteLine("[API Configuration] Platform: Desktop/Windows");
#endif

        services
            .ConfigureOptions(configuration)
            .ConfigureRefit(new Uri(apiUrl))
            .AddSingleton<IThemeService, ThemeService>();

        return services;
    }

    private static IServiceCollection ConfigureOptions(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions<ServerOption>()
            .Bind(configuration.GetSection(Server))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        return services;
    }
}
