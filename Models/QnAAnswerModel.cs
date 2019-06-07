using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QnABot.Models
{
    public class QnaResponse
    {
        public List<QnAAnswerModel> Answers { get; set; }
    }

    public class QnAAnswerModel
    {
        public string Answer { get; set; }
        public float Score { get; set; }
        public int Id { get; set; }
        public Context Context { get; set; }
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
