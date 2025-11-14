namespace VibeTravels.Infrastructure.Options;

internal sealed class OpenAiOptions
{
    public bool Enabled { get; init; }
    public string ApiKey { get; init; } = string.Empty;
    public string Model { get; init; } = "gpt-4o-mini";
    public double Temperature { get; init; } = 1;
    public int MaxTokens { get; init; } = 10000;
}

