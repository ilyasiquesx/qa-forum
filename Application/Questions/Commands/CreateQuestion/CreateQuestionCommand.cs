using MediatR;
using QAForum.Application.Common.Models;

namespace QAForum.Application.Questions.Commands.CreateQuestion
{
    public class CreateQuestionCommand : IRequest<Response<CreateQuestionCommandResponse>>
    {
        public string Title { get; set; }
        public string Content { get; set; }
    }
}