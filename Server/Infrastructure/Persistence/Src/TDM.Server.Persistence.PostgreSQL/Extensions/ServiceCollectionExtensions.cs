using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TDM.Domain.Common;

namespace TDM.Server.Persistence.PostgreSQL.Extensions;


/// <summary>
/// Extension methods to register infrastructure services in the service collection.
/// </summary>
public static class ServiceCollectionExtensions
{
    private const string POSTGRES_CONNECTION_NAME = "Postgres";
    
    /// <summary>
    /// Adds infrastructure-related services to the application's service collection.
    /// This includes security services, repository scanning, database context and distributed cache.
    /// </summary>
    /// <param name="services">The service collection to configure.</param>
    /// <param name="configuration">Application configuration used to read connection strings.</param>
    /// <returns>The updated service collection.</returns>
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .ConfigureDatabase(configuration)
            .ConfigureRepositories();
        return services;
    }
    
    /// <summary>
    /// Configures the Entity Framework Core DbContext using the Postgres connection string
    /// and registers IUnitOfWork resolved from the DbContext.
    /// </summary>
    /// <param name="services">The service collection to configure.</param>
    /// <param name="configuration">Application configuration used to read connection strings.</param>
    /// <returns>The updated service collection.</returns>
    private static IServiceCollection ConfigureDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        string postgresConnection = GetRequiredConnectionString(configuration, POSTGRES_CONNECTION_NAME);

        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(postgresConnection));

        services.AddScoped<IUnitOfWork>(provider => provider.GetRequiredService<AppDbContext>());

        return services;
    }
    
    /// <summary>
    /// Scans assemblies and registers repository implementations that implement IRepositoryMarker.
    /// </summary>
    /// <param name="services">The service collection to configure.</param>
    /// <returns>The updated service collection.</returns>
    private static IServiceCollection ConfigureRepositories(this IServiceCollection services)
    {
        services.Scan(s => s
            .FromAssemblies(
                typeof(IRepositoryMarker).Assembly,
                typeof(ServiceCollectionExtensions).Assembly)
            .AddClasses(c => c.AssignableTo<IRepositoryMarker>())
            .AsMatchingInterface()
            .WithScopedLifetime());

        return services;
    }
    
    /// <summary>
    /// Retrieves a required connection string from configuration by name.
    /// </summary>
    /// <param name="configuration">Application configuration used to read connection strings.</param>
    /// <param name="name">The name of the connection string to retrieve.</param>
    /// <returns>The connection string value.</returns>
    /// <exception cref="InvalidOperationException">Thrown when the requested connection string is missing or empty.</exception>
    private static string GetRequiredConnectionString(IConfiguration configuration, string name)
    {
        string? value = configuration.GetConnectionString(name);

        return string.IsNullOrWhiteSpace(value)
            ? throw new InvalidOperationException($"Connection string '{name}' is required but not found.")
            : value;
    }
}
/*public static IServiceCollection AddPersistenceServices(this IServiceCollection services, string connectionString)
{
    services.AddDbContext<AppDbContext>(options =>
    {
        options.UseNpgsql(connectionString, npgsqlOptions =>
        {
            npgsqlOptions.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName);
        });
    });

    services.AddScoped<IContactRepository, ContactRepository>();
    services.AddScoped<ITodoItemRepository, TodoItemRepository>();

    return services;
}*/
