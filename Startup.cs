using Bot.Builder.Community.Adapters.Alexa.Integration.AspNet.Core;
using Bot.Builder.Community.Adapters.Alexa.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Integration.AspNet.Core;
using Microsoft.Bot.Configuration;
using Microsoft.Bot.Connector.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StoryBot.Configuration;
using StoryBot.Dialogs;

namespace StoryBot
{
    public class Startup
    {
        private readonly IConfiguration _configuration;
        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            // Add the HttpClientFactory to be used for the QnAMaker calls.
            services.AddHttpClient();

            // Create the credential provider to be used with the Bot Framework Adapter.
            services.AddSingleton<ICredentialProvider, ConfigurationCredentialProvider>();

            // Create the Bot Framework Adapter with error handling enabled. 
            services.AddSingleton<IBotFrameworkHttpAdapter, AdapterWithErrorHandler>();

            // Create the storage we'll be using for User and Conversation state. (Memory is great for testing purposes.)
            services.AddSingleton<IStorage, MemoryStorage>();

            // Create the Conversation state. (Used by the Dialog system itself.)
            services.AddSingleton<ConversationState>();
            // The Dialog that will be run by the bot.
            services.AddSingleton<QnAStoryDialog>();

            // Create the bot as a transient. In this case the ASP Controller is expecting an IBot.
            services.AddTransient<IBot, Bots.StoryBot<QnAStoryDialog>>();
                        
            services.AddLuisService(_configuration.GetSection("LuisService").Get<LuisService>());
            services.AddQnAService();
            
            services.AddSingleton<IAlexaHttpAdapter>(_ =>
            {
                var alexaHttpAdapter = new AlexaHttpAdapter(validateRequests: true)
                {
                    OnTurnError = async (context, exception) =>
                    {
                        await context.SendActivityAsync("<say-as interpret-as=\"interjection\">boom</say-as>, explotó.");
                    },
                    ShouldEndSessionByDefault = false,
                    ConvertBotBuilderCardsToAlexaCards = true,
                };
                alexaHttpAdapter.Use(new AlexaIntentRequestToMessageActivityMiddleware(transformPattern: RequestTransformPatterns.MessageActivityTextFromSinglePhraseSlotValue));
                return alexaHttpAdapter;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseMvc();
        }
    }
}
