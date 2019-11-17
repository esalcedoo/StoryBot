using Microsoft.Bot.Builder.AI.Luis;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace StoryBot.Dialogs
{
    public class InterruptibleDialog : ComponentDialog
    {
        public LuisRecognizer _luisRecognizer;

        public InterruptibleDialog(string id, LuisRecognizer luisRecognizer, HelpDialog helpDialog, ResumeDialog resumeDialog)
            : base(id)
        {
            _luisRecognizer = luisRecognizer;
            AddDialog(helpDialog);
            AddDialog(resumeDialog);
        }

        protected override async Task<DialogTurnResult> OnBeginDialogAsync(DialogContext innerDc, object options, CancellationToken cancellationToken = default)
        {
            var result = await InterruptAsync(innerDc, cancellationToken);
            if (result != null)
            {
                return result;
            }

            return await base.OnBeginDialogAsync(innerDc, options, cancellationToken);
        }

        protected override async Task<DialogTurnResult> OnContinueDialogAsync(DialogContext innerDc, CancellationToken cancellationToken)
        {
            var result = await InterruptAsync(innerDc, cancellationToken);
            if (result != null)
            {
                return result;
            }

            return await base.OnContinueDialogAsync(innerDc, cancellationToken);
        }

        private async Task<DialogTurnResult> InterruptAsync(DialogContext innerDc, CancellationToken cancellationToken)
        {
            if (innerDc.Context.Activity.Type == ActivityTypes.Message)
            {
                var luisResult = await _luisRecognizer.RecognizeAsync(innerDc.Context, cancellationToken);

                IList<string> dialogNames = new List<string>() { "Help", "Resume" };

                var dialogName = dialogNames.FirstOrDefault(dn => dn.Equals(luisResult.Intents?.FirstOrDefault().Key, StringComparison.InvariantCultureIgnoreCase));

                if (!string.IsNullOrEmpty(dialogName) && !dialogName.Equals("none", StringComparison.CurrentCultureIgnoreCase))
                    return await innerDc.BeginDialogAsync(dialogName + "Dialog", cancellationToken);
            }

            return null;
        }
    }
}
