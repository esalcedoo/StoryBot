using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.AI.Luis;
using Microsoft.Bot.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StoryBot.Services;

namespace StoryBot.Configuration
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddQnAService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHttpClient<QnAService>(client =>
                {
                    QnAMakerService qnAMakerService = configuration.GetSection(nameof(QnAMakerService)).Get<QnAMakerService>() ?? new QnAMakerService();

                    qnAMakerService.Validate();

                    client.BaseAddress = new Uri($"{qnAMakerService.Hostname}/knowledgebases/{qnAMakerService.KbId}/");
                    client.DefaultRequestHeaders.Add("Authorization", $"EndpointKey {qnAMakerService.EndpointKey}");
                }
            );


            LuisService luisService = configuration.GetSection(nameof(LuisService)).Get<LuisService>();
            luisService.Validate();

            var app = new LuisApplication(luisService);

            var recognizer = new LuisRecognizer(app);
            services.AddSingleton(recognizer);

            return services;
        }
    }
}
