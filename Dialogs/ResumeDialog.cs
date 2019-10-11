using Microsoft.Bot.Builder.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace StoryBot.Dialogs
{
    public class ResumeDialog : Dialog
    {
        public ResumeDialog() : base(nameof(ResumeDialog))
        {
        }

        public override Task<DialogTurnResult> BeginDialogAsync(DialogContext dc, object options = null, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
