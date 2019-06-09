// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Logging;
using StoryBot.Models;
using StoryBot.Services;

namespace StoryBot.Bots
{
    public class StoryBot : ActivityHandler
    {
        private readonly ILogger<StoryBot> _logger;
        private readonly QnAService _qnAService;

        public StoryBot(ILogger<StoryBot> logger, QnAService qnaService)
        {
            _logger = logger;
            _qnAService = qnaService;
        }

        protected override async Task OnMessageActivityAsync(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Calling QnA Maker");

            QnAAnswerModel response = await _qnAService
                .GenerateAnswer(turnContext.Activity.Text, turnContext.Activity.Locale);

            if (response != null)
            {
                IEnumerable<string> suggestedActions = response.Context?.Prompts?.Select(p => p.DisplayText);

                if (suggestedActions != null)
                {
                    await turnContext.SendActivityAsync(
                        MessageFactory.SuggestedActions(
                            response.Context?.Prompts?.Select(p => p.DisplayText),
                            response.Answer), cancellationToken);
                }
                else
                {
                    await turnContext.SendActivityAsync(
                        MessageFactory.Text(response.Answer), cancellationToken);
                }
            }
            else
            {
                await turnContext.SendActivityAsync(MessageFactory.Text("No QnA Maker answers were found."), cancellationToken);
            }
        }
    }
}
