using Newtonsoft.Json;
using StoryBot.Converters;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace StoryBot.Services.QnA
{
    public class QnaResponse
    {
        public List<QnAAnswerModel> Answers { get; set; }
    }

    public class QnAAnswerModel
    {
        [JsonConverter(typeof(AnswerJsonConverter))]
        public Answer Answer { get; set; }
        public float Score { get; set; }
        public int Id { get; set; }
        public Context Context { get; set; }
    }

    public class Answer
    {
        public string SSML { get; set; }
        public string Text { get; set; }
        public string Image { get; set; }
        public string Video { get; set; }
    }

    public class Context
    {
        public bool IsContextOnly { get; set; }
        public List<Prompt> Prompts { get; set; }
    }

    public class Prompt
    {
        public int QnaId { get; set; }
        public string DisplayText { get; set; }
    }
}
