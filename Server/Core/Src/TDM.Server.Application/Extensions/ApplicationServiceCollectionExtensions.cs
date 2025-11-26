using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using TDM.Server.Application.Common;
using TDM.Server.Application.Services;
using TDM.Server.Persistence.PostgreSQL;

namespace TDM.Server.Application.Extensions;

public static class ApplicationServiceCollectionExtensions
{
    /// <summary>
    /// Registers application-level services including MediatR and FluentValidation.
    /// </summary>
    /// <param name="services">The service collection to configure.</param>
    /// <returns>The updated service collection.</returns>
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // Регистрация MediatR
        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssemblies(typeof(ApplicationServiceCollectionExtensions).Assembly);
        });

        // Регистрация FluentValidation валидаторов
        services.AddValidatorsFromAssembly(typeof(ApplicationServiceCollectionExtensions).Assembly);

        // Регистрация сервисов
        services.ConfigureServices();

        return services;
    }

    /// <summary>
    /// Registers application services.
    /// </summary>
    /// <param name="services">The service collection to configure.</param>
    /// <returns>The updated service collection.</returns>
    private static IServiceCollection ConfigureServices(this IServiceCollection services)
    {
        services.AddSingleton<IPasswordHasherService, PasswordHasherService>();

        return services;
    }

    /// <summary>
    /// Configures and registers Serilog logging for the application.
    /// Reads logging configuration from the application's configuration source.
    /// </summary>
    /// <param name="host">The host builder to configure.</param>
    public static void ConfigureSerilog(this IHostBuilder host)
    {
        host.UseSerilog((context, configuration) =>
            configuration.ReadFrom.Configuration(context.Configuration));
    }

    /// <summary>
    /// Applies pending Entity Framework Core migrations to the database.
    /// Creates a service scope to retrieve the database context and executes all unapplied migrations.
    /// </summary>
    /// <param name="app">The application builder to retrieve services from.</param>
    public static void ApplyMigrations(this WebApplication app)
    {
        using IServiceScope scope = app.Services.CreateScope();

        using AppDbContext context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        context.Database.Migrate();
    }
    
    /// <summary>
    /// Инициализирует базу данных начальными данными, если она пуста.
    /// Должен вызываться после применения миграций.
    /// </summary>
    /// <param name="app">The application builder to retrieve services from.</param>
    public static void InitializeDatabase(this WebApplication app)
    {
        using IServiceScope scope = app.Services.CreateScope();

        using AppDbContext context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        DbInitializer.Initialize(context);
    }
}

