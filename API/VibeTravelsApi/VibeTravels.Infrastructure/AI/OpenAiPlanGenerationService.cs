using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OpenAI.Chat;
using VibeTravels.Core.AI;
using VibeTravels.Infrastructure.Options;

namespace VibeTravels.Infrastructure.AI;

internal sealed class OpenAiPlanGenerationService(
    IOptions<OpenAiOptions> options,
    ILogger<OpenAiPlanGenerationService> logger) : IAiPlanGeneratorService
{
    private readonly OpenAiOptions aiOptions = options.Value;

    public async Task<string> GeneratePlanAsync(string inputPayload, CancellationToken cancellationToken = default)
    {
        try
        {
            logger.LogInformation("Starting OpenAI plan generation with model {Model}", aiOptions.Model);

            ChatClient client = new(model: aiOptions.Model, apiKey: aiOptions.ApiKey);

            string systemPrompt = BuildSystemPrompt();
            string userPrompt = BuildUserPrompt(inputPayload);

            ChatCompletion completion = await client.CompleteChatAsync(
                [
                    new SystemChatMessage(systemPrompt),
                    new UserChatMessage(userPrompt)
                ],
                new ChatCompletionOptions
                {
                    Temperature = (float)aiOptions.Temperature,
                    MaxOutputTokenCount = aiOptions.MaxTokens
                },
                cancellationToken);

            string result = completion.Content[0].Text;

            logger.LogInformation("Successfully generated plan with {TokenCount} tokens", completion.Usage.OutputTokenCount);

            return result;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to generate plan using OpenAI");
            throw;
        }
    }

    private static string BuildSystemPrompt()
    {
        return """
            You are an expert travel planner AI assistant for VibeTravels.
            Your task is to generate detailed, personalized travel itineraries based on user input.
            
            Guidelines:
            - Generate plans in Markdown format
            - If travel days are specified, create a day-by-day itinerary (Day 1, Day 2, etc.)
            - If travel days are NOT specified, create an activity list without daily structure
            - Consider the user's travel preferences (style, accommodation, climate) if provided
            - Include specific activities, attractions, and practical tips
            - Be concise but informative
            - Focus on the destination mentioned in the note's location
            - Respect the number of travelers and adjust recommendations accordingly
            - If a start date is provided, consider seasonal aspects
            
            Format:
            - Use clear headings (##, ###)
            - Use bullet points and numbered lists where appropriate
            - Keep descriptions succinct but engaging
            - Include practical tips at the end if relevant
            """;
    }

    private static string BuildUserPrompt(string inputPayload)
    {
        return $"""
            Generate a travel plan based on the following information:
            
            ```json
            {inputPayload}
            ```
            
            Please provide the complete travel plan in Markdown format.
            If "TravelDays" is specified, structure it as a day-by-day itinerary.
            Otherwise, provide it as a curated list of activities and recommendations.
            """;
    }
}

