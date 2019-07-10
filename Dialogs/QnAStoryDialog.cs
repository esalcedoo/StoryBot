using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.AI.Luis;
using Microsoft.Bot.Builder.Dialogs;
using StoryBot.Models;
using StoryBot.Services;

namespace StoryBot.Dialogs
{
    public class QnAStoryDialog : InterruptibleDialog
    {
        private readonly QnAService _qnAService;
        public QnAStoryDialog(QnAService qnaService, LuisRecognizer luisRecognizer) : base(nameof(QnAStoryDialog), luisRecognizer)
        {
            _qnAService = qnaService;

            AddDialog(new WaterfallDialog(nameof(WaterfallDialog), new WaterfallStep[]
            {
                QnADialogAsync,
            }));
            InitialDialogId = nameof(WaterfallDialog);
        }

        public async Task<DialogTurnResult> QnADialogAsync(DialogContext dc, CancellationToken cancellationToken = default)
        {
            // Call QnA Maker and get results.
            QnAAnswerModel qnaResult = await _qnAService.GenerateAnswer(dc.Context.Activity.Text, dc.Context.Activity.Locale);

            if (qnaResult == null)
            {
                // No answer found.
                await dc.Context.SendActivityAsync("No contemplamos esa opción. Te lo volveré a preguntar:");
                // TODO Get Last Question Asked & reprompt it
            }
            else
            {
                // Respond with QnA result.
                IEnumerable<string> suggestedActions = qnaResult.Context?.Prompts?.Select(p => p.DisplayText);

                if (suggestedActions != null)
                {
                    await dc.Context.SendActivityAsync(
                        MessageFactory.SuggestedActions(
                            suggestedActions, qnaResult.Answer.Text,
                            qnaResult.Answer.SSML), cancellationToken);
                }
                else
                {
                    await dc.Context.SendActivityAsync(
                        MessageFactory.Text(qnaResult.Answer.Text, qnaResult.Answer.SSML), cancellationToken);
                }
            }
            return await dc.EndDialogAsync(cancellationToken: cancellationToken);
        }
    }
}
