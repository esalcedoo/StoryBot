using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Bot.Builder.Community.Adapters.Alexa;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.AI.Luis;
using Microsoft.Bot.Builder.Dialogs;
using StoryBot.Services;
using StoryBot.Services.QnA;

namespace StoryBot.Dialogs
{
    public class QnAStoryDialog : InterruptibleDialog
    {
        private readonly QnAService _qnAService;
        private QnAAnswerModel _qnaResult;

        public QnAStoryDialog(QnAService qnaService, LuisRecognizer luisRecognizer) : base(nameof(QnAStoryDialog), luisRecognizer)
        {
            _qnAService = qnaService;

            AddDialog(new WaterfallDialog(nameof(WaterfallDialog), new WaterfallStep[]
            {
                GetQnAAsync,
                SendVideoAsync,
                SendQuestionAsync
            }));
            InitialDialogId = nameof(WaterfallDialog);
        }

        public async Task<DialogTurnResult> GetQnAAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken = default)
        {
            // Call QnA Maker and get results.
            _qnaResult = await _qnAService.GenerateAnswer(stepContext.Context.Activity.Text, stepContext.Context.Activity.Locale);

            if (_qnaResult == null)
            {
                // No answer found.
                await stepContext.Context.SendActivityAsync("No contemplamos esa opción.");
                return await stepContext.EndDialogAsync();
            }
            return await stepContext.NextAsync();
        }

        private async Task<DialogTurnResult> SendVideoAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            if (_qnaResult.Answer.Video != null)
            {
                if (stepContext.Context.Activity.ChannelId.Equals("Alexa", StringComparison.InvariantCultureIgnoreCase) 
                    && stepContext.Context.AlexaDeviceHasDisplay())
                {
                    stepContext.Context.AlexaResponseDirectives().Add(_qnaResult.ToVideoDirective());
                }
                var activity = _qnaResult.ToVideoActivity();
                
                await stepContext.Context.SendActivityAsync(activity, cancellationToken);
            }
            return await stepContext.NextAsync();
        }

        private async Task<DialogTurnResult> SendQuestionAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            if (!string.IsNullOrEmpty(_qnaResult.Answer.Text))
            {
                if (stepContext.Context.Activity.ChannelId.Equals("Alexa", StringComparison.InvariantCultureIgnoreCase)
                        && stepContext.Context.AlexaDeviceHasDisplay())
                {
                    stepContext.Context.AlexaResponseDirectives().Add(_qnaResult.ToAlexaDirective());
                }
                await stepContext.Context.SendActivityAsync(_qnaResult.ToActivity(), cancellationToken);
            }
            return await stepContext.EndDialogAsync();
        }
    }
}
