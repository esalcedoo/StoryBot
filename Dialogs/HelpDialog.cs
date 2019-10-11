using Bot.Builder.Community.Adapters.Alexa;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
using StoryBot.Services.Alexa;
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
            IMessageActivity activity = MessageFactory.SuggestedActions(
                        new List<CardAction>()
                        {
                new CardAction(type: ActionTypes.ImBack, title: "Intro", displayText: "Intro", value:"intro"),
                new CardAction(type: ActionTypes.ImBack, title: "Comenzar", displayText: "Comenzar", value:"Comenzar")
                        },
                        text: "Este juego consiste en que vayamos construyendo tu propia aventura. Yo narraré un tramo de la historia, y tú me dirás cómo continuar.");

            if (dc.Context.Activity.ChannelId.Equals("Alexa", StringComparison.InvariantCultureIgnoreCase)
                && dc.Context.AlexaDeviceHasDisplay())
            {
                activity.InputHint = InputHints.AcceptingInput;
                dc.Context.AlexaResponseDirectives().Add(activity.ToAlexaDirective());
            }

            await dc.Context.SendActivityAsync(activity, cancellationToken);
            return await dc.EndDialogAsync();
        }
    }
}
