using StoryBot.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class DialogCollectionExtensions
    {
        public static IServiceCollection AddDialogs(this IServiceCollection services)
        {
            // The Dialog that will be run by the bot.
            services.AddTransient<QnAStoryDialog>();

            // Other Dialogs
            services.AddTransient<HelpDialog>();
            services.AddTransient<ResumeDialog>();

            return services;
        }
    }
}
