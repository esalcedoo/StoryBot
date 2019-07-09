using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bot.Builder.Community.Adapters.Alexa.Integration.AspNet.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Bot.Builder;

namespace StoryBot.Controllers
{
    [Route("api/skillrequests")]
    [ApiController]
    public class AlexaSkillController : ControllerBase
    {
        private readonly IAlexaHttpAdapter _adapter;
        private readonly IBot _bot;

        public AlexaSkillController(IAlexaHttpAdapter adapter, IBot bot)
        {
            _adapter = adapter;
            _bot = bot;
        }

        [HttpPost]
        public async Task PostAsync()
        {
            await _adapter.ProcessAsync(Request, Response, _bot);
        }
    }
}