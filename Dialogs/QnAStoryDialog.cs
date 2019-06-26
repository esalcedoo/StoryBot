using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using StoryBot.Models;
using StoryBot.Services;

namespace StoryBot.Dialogs
{
    public class QnAStoryDialog : Dialog
    {
        private readonly QnAService _QnAService;
        public QnAStoryDialog(QnAService qnaService) : base(nameof(QnAStoryDialog))
        {
            _QnAService = qnaService;
        }

        public override async Task<DialogTurnResult> BeginDialogAsync(DialogContext dc, object options = null,
            CancellationToken cancellationToken = new CancellationToken())
        {
            // Call QnA Maker and get results.
            QnAAnswerModel qnaResult = await _QnAService.GenerateAnswer(dc.Context.Activity.Text, dc.Context.Activity.Locale);

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
