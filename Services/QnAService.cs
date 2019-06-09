using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using StoryBot.Models;

namespace StoryBot.Services
{
    public class QnAService
    {
        private readonly HttpClient _client;

        public QnAService(HttpClient client)
        {
            _client = client;
        }

        internal async Task<QnAAnswerModel> GenerateAnswer(string message, string lang)
        {
            QnAAnswerModel qnAResponse = null;

            string route = $"generateAnswer";

            var requestBody = JsonConvert.SerializeObject(new { question = message });

            HttpContent httpContent = new StringContent(requestBody, Encoding.UTF8, "application/json");

            var response = await _client.PostAsync(route, httpContent);

            if (response.IsSuccessStatusCode)
            {
                qnAResponse = (await response.Content.ReadAsAsync<QnaResponse>()).Answers.FirstOrDefault();
            }

            return qnAResponse;
        }
    }
}
