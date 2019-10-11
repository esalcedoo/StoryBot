// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Microsoft.Bot.Builder.Integration.AspNet.Core;
using Microsoft.Bot.Connector.Authentication;
using Microsoft.Extensions.Logging;

namespace StoryBot
{
    public class AdapterWithErrorHandler : BotFrameworkHttpAdapter
    {
        public AdapterWithErrorHandler(ICredentialProvider credentialProvider, ILogger<BotFrameworkHttpAdapter> logger)
            : base(credentialProvider)
        {
            // Enable logging at the adapter level using OnTurnError.
            OnTurnError = async (turnContext, exception) =>
            {
                logger.LogError($"Exception caught : {exception}");
                await turnContext.SendActivityAsync("Algo ha ido mal.");
            };
        }
    }
}
