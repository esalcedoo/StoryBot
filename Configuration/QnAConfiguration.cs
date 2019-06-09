using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StoryBot.Configuration
{
    public class QnAConfiguration
    {
        public string QnAKnowledgebaseId { get; set; }
        public string QnASubscriptionKey { get; set; }
        public string QnAAuthKey { get; set; }
        public string QnAEndpointHostName { get; set; }
    }
}
