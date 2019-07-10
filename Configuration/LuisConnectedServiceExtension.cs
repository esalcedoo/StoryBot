using Microsoft.Bot.Configuration;
using System;

namespace StoryBot.Configuration
{
    public static class LuisConnectedServiceExtension
    {
        public static void Validate(this LuisService qnaService)
        {
            if (string.IsNullOrWhiteSpace(qnaService.AppId))
            {
                throw new InvalidOperationException("The LUIS AppId ('appId') is required to run this sample.");
            }

            if (string.IsNullOrWhiteSpace(qnaService.AuthoringKey))
            {
                throw new InvalidOperationException("The LUIS AuthoringKey ('authoringKey') is required to run this sample.");
            }

            if (string.IsNullOrWhiteSpace(qnaService.SubscriptionKey))
            {
                throw new InvalidOperationException("The LUIS SubscriptionKey ('subscriptionKey') is required to run this sample.");
            }
        }
    }
}
