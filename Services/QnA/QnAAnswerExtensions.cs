using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using System.Collections.Generic;
using System.Linq;

namespace StoryBot.Services.QnA
{
    public static class QnAAnswerExtensions
    {
        internal static IActivity ToActivity(this QnAAnswerModel qnAAnswer)
        {
            IEnumerable<string> suggestedActions = qnAAnswer.Context?.Prompts?.Select(p => p.DisplayText);
            IMessageActivity activity;
            if (suggestedActions != null)
            {
                activity = MessageFactory.SuggestedActions(suggestedActions, qnAAnswer.Answer.Text, qnAAnswer.Answer.SSML);
            }
            else
            {
                activity = MessageFactory.Text(qnAAnswer.Answer.Text, qnAAnswer.Answer.SSML);
            }

            activity.InputHint = InputHints.ExpectingInput;
            return activity;
        }

        internal static IActivity ToVideoActivity(this QnAAnswerModel qnAAnswer)
        {
            VideoCard videoCard = new VideoCard()
            {
                Title = "Video",
                Subtitle = string.Empty,
                Image = new ThumbnailUrl
                {
                    Url = qnAAnswer.Answer.Video,
                },
                Media = new List<MediaUrl>
                {
                    new MediaUrl()
                    {
                        Url = qnAAnswer.Answer.Video,
                    },
                }
            };
            IMessageActivity activity = MessageFactory.Attachment(videoCard.ToAttachment());
            activity.InputHint = InputHints.AcceptingInput;
            return activity;
        }
    }
}
