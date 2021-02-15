using MediatR;
using QAForum.Application.Common.Models;

namespace QAForum.Application.Questions.Commands.UpdateQuestion
{
    public class UpdateQuestionCommand : IRequest<Response<object>>
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }

        public long? BestAnswerId { get; set; }
    }
}