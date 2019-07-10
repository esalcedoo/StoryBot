using Microsoft.Bot.Builder.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace StoryBot.Dialogs
{
    public class HelpDialog : Dialog
    {
        public HelpDialog() : base(nameof(HelpDialog))
        {
        }

        public override async Task<DialogTurnResult> BeginDialogAsync(DialogContext dc, object options = null, CancellationToken cancellationToken = default)
        {
            await dc.Context.SendActivityAsync("Este juego consiste en que vayamos construyendo tu propia aventura. Yo narraré un tramo de la historia, y tú me dirás cómo continuar.");
            return await dc.EndDialogAsync();
        }
    }
}
