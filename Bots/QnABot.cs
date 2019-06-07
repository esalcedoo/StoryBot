// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Logging;
using QnABot.Models;
using QnABot.Services;

namespace QnABot.Bots
{
    public class QnABot : ActivityHandler
    {
        private readonly ILogger<QnABot> _logger;
        private readonly QnAService _qnAService;

        public QnABot(ILogger<QnABot> logger, QnAService qnaService)
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
                await turnContext.SendActivityAsync(MessageFactory.Text(response.Answer), cancellationToken);
            }
            else
            {
                await turnContext.SendActivityAsync(MessageFactory.Text("No QnA Maker answers were found."), cancellationToken);
            }
        }
    }
}
