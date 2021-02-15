using System.Collections.Generic;
using QAForum.Application.Questions.Models;

namespace QAForum.Application.Questions.Queries.GetQuestions
{
    public class GetQuestionsQueryResponse
    {
        public IEnumerable<QuestionIndexModel> Questions { get; set; }
        public int Pages { get; set; }
        public int Page { get; set; }
    }
}