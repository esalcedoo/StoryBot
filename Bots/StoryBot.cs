﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Logging;
using StoryBot.Services;
namespace StoryBot.Bots
{
    public class StoryBot<T> : ActivityHandler where T : Dialog
    {
        protected readonly Dialog _dialog;
        private readonly ILogger<StoryBot<T>> _logger;
        protected readonly BotState _conversationState;

        public StoryBot(T dialog, ILogger<StoryBot<T>> logger, ConversationState conversationState)
        {
            _logger = logger;
            _conversationState = conversationState;
            _dialog = dialog;
        }

        public override async Task OnTurnAsync(ITurnContext turnContext, CancellationToken cancellationToken = default)
        {
            await base.OnTurnAsync(turnContext, cancellationToken);

            // Save any state changes that might have occured during the turn.
            await _conversationState.SaveChangesAsync(turnContext, false, cancellationToken);
        }
        
        protected override async Task OnMembersAddedAsync(IList<ChannelAccount> membersAdded, ITurnContext<IConversationUpdateActivity> turnContext, CancellationToken cancellationToken)
        {
            foreach (var member in membersAdded)
            {
                if (member.Id != turnContext.Activity.Recipient.Id)
                {
                    await turnContext.SendActivityAsync(
                        MessageFactory.SuggestedActions(
                            new List<CardAction>()
                            {
                                new CardAction(type: ActionTypes.ImBack, title: "Crea mi propia aventura", displayText: "Comenzar", value:"Crea mi propia aventura"),
                                new CardAction(type: ActionTypes.ImBack, title: "Ayuda", displayText: "Ayuda", value:"Ayuda")
                            },
                            text: "¡Hola! Soy el Narrador de Historias"),
                        cancellationToken);
                }
            }
        }

        protected override async Task OnMessageActivityAsync(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Running dialog with Message Activity.");

            DialogSet dialogSet = new DialogSet(_conversationState.CreateProperty<DialogState>("StoryState"));

            dialogSet.Add(_dialog);
            DialogContext dialogContext = await dialogSet.CreateContextAsync(turnContext, cancellationToken);
            DialogTurnResult results = await dialogContext.ContinueDialogAsync(cancellationToken);

            if (results.Status == DialogTurnStatus.Empty)
            {
                await dialogContext.BeginDialogAsync(_dialog.Id, null, cancellationToken);
            }
        }
        
    }
}
