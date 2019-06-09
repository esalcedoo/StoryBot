using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
                    QnAMakerService qnAMakerService = configuration.GetSection(nameof(QnAMakerService)).Get<QnAMakerService>();

                    qnAMakerService.Validate();

                    client.BaseAddress = new Uri($"{qnAMakerService.Hostname}/knowledgebases/{qnAMakerService.KbId}/");
                    client.DefaultRequestHeaders.Add("Authorization", $"EndpointKey {qnAMakerService.EndpointKey}");
                }
            );
            return services;
        }
    }
}
