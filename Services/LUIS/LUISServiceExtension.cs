using Microsoft.Bot.Configuration;
using System;

namespace StoryBot.Configuration
{
    public static class LuisServiceExtension
    {
        public static void Validate(this LuisService luisService)
        {
            if (string.IsNullOrWhiteSpace(luisService.AppId))
            {
                throw new InvalidOperationException("The LUIS AppId ('appId') is required to run this sample.");
            }

            if (string.IsNullOrWhiteSpace(luisService.AuthoringKey))
            {
                throw new InvalidOperationException("The LUIS AuthoringKey ('authoringKey') is required to run this sample.");
            }

            if (string.IsNullOrWhiteSpace(luisService.SubscriptionKey))
            {
                throw new InvalidOperationException("The LUIS SubscriptionKey ('subscriptionKey') is required to run this sample.");
            }
        }

        public static bool IsValid(this LuisService luisService)
        {
            return !(string.IsNullOrWhiteSpace(luisService.AppId)
                || string.IsNullOrWhiteSpace(luisService.AuthoringKey)
                || string.IsNullOrWhiteSpace(luisService.SubscriptionKey));
        }
    }
}
