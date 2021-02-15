using System;
using QAForum.Application.Users.Models;

namespace QAForum.Application.Answers.Queries.GetAnswer
{
    public class GetAnswerQueryResponse
    {
        public long Id { get; set; }
        
        public string Content { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public UserViewModelExtended CreatedBy { get; set; }
    }
}