using System;
using System.Collections.Generic;
using QAForum.Application.Answers.Queries.GetAnswer;
using QAForum.Application.Users.Models;

namespace QAForum.Application.Questions.Queries.GetQuestion
{
    public class GetQuestionQueryResponse
    {
        public long Id { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public GetAnswerQueryResponse BestAnswer { get; set; }

        public IList<GetAnswerQueryResponse> Answers { get; set; } = new List<GetAnswerQueryResponse>();

        public UserViewModelExtended CreatedBy { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }
}