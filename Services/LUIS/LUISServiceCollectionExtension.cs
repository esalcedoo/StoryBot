using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.AI.Luis;
using Microsoft.Bot.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StoryBot.Services.QnA;

namespace StoryBot.Configuration
{
    public static class LUISServiceCollectionExtension
    {
        public static IServiceCollection AddLuisService(this IServiceCollection services, Action<LuisService> setup = null)
        {
            LuisService luisService = new LuisService();

            setup?.Invoke(luisService);
            services.AddLuisService(luisService);

            return services;
        }

        public static IServiceCollection AddLuisService(this IServiceCollection services, LuisService luisService)
        {
            luisService.Validate();

            var app = new LuisApplication(luisService);

            var recognizer = new LuisRecognizer(app);
            services.AddSingleton(recognizer);

            return services;
        }
    }
}
