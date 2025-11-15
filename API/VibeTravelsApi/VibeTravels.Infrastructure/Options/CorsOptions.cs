namespace VibeTravels.Infrastructure.Options;

internal sealed class CorsOptions
{
    public string[] AllowedOrigins { get; set; } = [];
}