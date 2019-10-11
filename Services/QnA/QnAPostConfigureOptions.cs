using Microsoft.Bot.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace StoryBot.Configuration
{
    internal class QnAPostConfigureOptions : IPostConfigureOptions<QnAMakerService>
    {
        private readonly IConfiguration _configuration;

        public QnAPostConfigureOptions(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void PostConfigure(string name, QnAMakerService options)
        {
            if (!options.IsValid())
            {
                var settings = _configuration.GetSection("QnAMakerService").Get<QnAMakerService>();
                options.KbId = settings.KbId;
                options.EndpointKey = settings.EndpointKey;
                options.Hostname = settings.Hostname;
            }
        }
    }
}