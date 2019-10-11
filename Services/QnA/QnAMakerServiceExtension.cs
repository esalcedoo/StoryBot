using System;
using Microsoft.Bot.Configuration;

namespace StoryBot.Configuration
{
    public static class QnAMakerServiceExtension
    {
        public static void Validate(this QnAMakerService qnaService)
        {
            if (string.IsNullOrWhiteSpace(qnaService.KbId))
            {
                throw new InvalidOperationException("The QnA KnowledgeBaseId ('kbId') is required to run this sample. Please update your 'appsettings.json' file.");
            }

            if (string.IsNullOrWhiteSpace(qnaService.EndpointKey))
            {
                throw new InvalidOperationException("The QnA EndpointKey ('endpointKey') is required to run this sample. Please update your 'appsettings.json' file.");
            }

            if (string.IsNullOrWhiteSpace(qnaService.Hostname))
            {
                throw new InvalidOperationException("The QnA Host ('hostname') is required to run this sample. Please update your 'appsettings.json' file.");
            }
        }

        public static bool IsValid(this QnAMakerService qnaService)
        {
            return !(string.IsNullOrWhiteSpace(qnaService.KbId)
                || string.IsNullOrWhiteSpace(qnaService.EndpointKey)
                || string.IsNullOrWhiteSpace(qnaService.Hostname));
        }
    }
}
