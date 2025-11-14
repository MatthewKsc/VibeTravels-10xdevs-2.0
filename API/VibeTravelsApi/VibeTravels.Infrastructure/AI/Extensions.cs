﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VibeTravels.Core.AI;
using VibeTravels.Infrastructure.Options;

namespace VibeTravels.Infrastructure.AI;

public static class Extensions
{
    public static void AddAiConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<OpenAiOptions>(configuration.GetSection("OpenAi"));

        var openAiOptions = configuration.GetSection("OpenAi").Get<OpenAiOptions>();

        if (openAiOptions?.Enabled == true)
        {
            services.AddScoped<IAiPlanGeneratorService, OpenAiPlanGenerationService>();
        }
        else
        {
            services.AddScoped<IAiPlanGeneratorService, MockAiPlanGeneratorService>();
        }
    }
}