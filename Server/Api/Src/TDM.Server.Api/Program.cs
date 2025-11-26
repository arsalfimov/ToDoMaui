using System.Text.Json.Serialization;
using TDM.Server.Application.Extensions;
using TDM.Server.Middleware.Extensions;
using TDM.Server.Persistence.PostgreSQL.Extensions;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Host.ConfigureSerilog();

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        // Добавляем конвертер для enum - сериализация/десериализация как строки
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    });
    
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services
    .AddInfrastructure(builder.Configuration)
    .AddApplication()
    .AddMiddleware();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", corsPolicyBuilder =>
    {
        corsPolicyBuilder
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

WebApplication app = builder.Build();

app.UseExceptionHandling();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.ApplyMigrations();
    app.InitializeDatabase();
}

app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseAuthorization();
app.MapControllers();

await app.RunAsync();

// Make the implicit Program class public for integration testing
namespace TDM.Server.API
{
    public partial class Program { }
}
