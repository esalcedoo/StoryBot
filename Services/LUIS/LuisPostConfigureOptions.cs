using Microsoft.Bot.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace StoryBot.Configuration
{
    internal class LuisPostConfigureOptions : IPostConfigureOptions<LuisService>
    {
        private readonly IConfiguration _configuration;

        public LuisPostConfigureOptions(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void PostConfigure(string name, LuisService options)
        {
            if (!options.IsValid())
            {
                var settings = _configuration.GetSection("LuisService").Get<LuisService>();
                options.AppId = settings.AppId;
                options.AuthoringKey = settings.AuthoringKey;
                options.SubscriptionKey = settings.SubscriptionKey;
            }
        }
    }
}