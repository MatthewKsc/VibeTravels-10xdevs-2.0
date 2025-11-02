using Serilog;
using VibeTravels.Api.Endpoints;
using VibeTravels.Application;
using VibeTravels.Core;
using VibeTravels.Infrastructure;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.AddInfrastructureBuilderConfig();
builder.Services
    .AddCore()
    .AddApplication()
    .AddInfrastructure(builder.Configuration);

WebApplication app = builder.Build();

app.UseInfrastructure();

RouteGroupBuilder api = app.MapGroup("/api");
api.MapUserEndpoints();

try
{
    Log.Information("Starting VibeTravels API");
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application startup failed");
}
finally
{
    Log.CloseAndFlush();
}