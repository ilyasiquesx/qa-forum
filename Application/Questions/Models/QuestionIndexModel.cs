using System;
using QAForum.Application.Users.Models;

namespace QAForum.Application.Questions.Models
{
    public class QuestionIndexModel
    {
        public long Id { get; set; }

        public string Title { get; set; }

        public int AnswersCount { get; set; }

        public UserViewModel CreatedBy { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }
}