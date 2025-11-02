using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Events;
using VibeTravels.Shared.CQRS;

namespace VibeTravels.Infrastructure.Logging;

internal static class Extensions
{
    private const string SerilogFolderName = "Logs";
    private const string SerilogTimeFormat = "yyyy_MM_dd";
    private const string SerilogTemplate = "{Timestamp:yyyy-MM-dd HH:mm:ss.fff} [{Level:u3}] {SourceContext} {Message:lj}{NewLine}{Exception}";
    
    public static WebApplicationBuilder ConfigureSerilogLogging(this WebApplicationBuilder builder)
    {
        string logDirectory = Path.Combine(builder.Environment.ContentRootPath, SerilogFolderName);
        Directory.CreateDirectory(logDirectory);

        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(builder.Configuration)
            .Enrich.FromLogContext()
            .Enrich.WithMachineName()
            .Enrich.WithThreadId()
            .WriteTo.Console()
            .WriteTo.Map(
                keySelector: (LogEvent e) => e.Timestamp.ToString(SerilogTimeFormat),
                configure: (date, cfg) =>
                    cfg.File(
                        path: Path.Combine(logDirectory, $"logs_{date}.txt"),
                        shared: true,
                        retainedFileCountLimit: 30,
                        outputTemplate: SerilogTemplate
                    )
            )
            .CreateLogger();

        builder.Host.UseSerilog();
        return builder;
    }
    
    public static void AddCommandHandlerLogging(this IServiceCollection services) =>
        services.Decorate(typeof(ICommandHandler<>), typeof(LoggingCommandHandlerDecorator<>));

    public static void AddQueryHandlerLogging(this IServiceCollection services) =>
        services.Decorate(typeof(IQueryHandler<,>), typeof(LoggingQueryHandlerDecorator<,>));
}