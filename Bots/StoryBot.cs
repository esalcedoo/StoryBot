// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Logging;
using StoryBot.Dialogs;
using StoryBot.Models;
using StoryBot.Services;

namespace StoryBot.Bots
{
    public class StoryBot<T> : ActivityHandler where T : Dialog
    {
        protected readonly Dialog _dialog;
        private readonly ILogger<StoryBot<T>> _logger;
        private readonly QnAService _qnAService;
        protected readonly BotState _conversationState;

        public StoryBot(T dialog, ILogger<StoryBot<T>> logger, QnAService qnaService, ConversationState conversationState)
        {
            _logger = logger;
            _qnAService = qnaService;
            _conversationState = conversationState;
            _dialog = dialog;
        }
        public override async Task OnTurnAsync(ITurnContext turnContext, CancellationToken cancellationToken = default(CancellationToken))
        {
            await base.OnTurnAsync(turnContext, cancellationToken);

            // Save any state changes that might have occured during the turn.
            await _conversationState.SaveChangesAsync(turnContext, false, cancellationToken);
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
