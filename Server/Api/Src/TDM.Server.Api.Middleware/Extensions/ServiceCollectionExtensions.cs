using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using TDM.Server.Middleware.Middleware;

namespace TDM.Server.Middleware.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMiddleware(this IServiceCollection services)
    {
        services.AddScoped<ExceptionHandlingMiddleware>();

        return services;
    }

    public static IApplicationBuilder UseExceptionHandling(this IApplicationBuilder app)
    {
        return app.UseMiddleware<ExceptionHandlingMiddleware>();
    }
}
