using VibeTravels.Api.Endpoints;
using VibeTravels.Application;
using VibeTravels.Core;
using VibeTravels.Infrastructure;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddCore()
    .AddApplication()
    .AddInfrastructure(builder.Configuration);

WebApplication app = builder.Build();

app.UseInfrastructure();

RouteGroupBuilder api = app.MapGroup("/api");
api.MapUserEndpoints();

app.Run();