using MediatR;
using QAForum.Application.Common.Models;

namespace QAForum.Application.Answers.Commands.CreateAnswer
{
    public class CreateAnswerCommand : IRequest<Response<CreateAnswerCommandResponse>>
    {
        public long QuestionId { get; set; }
        
        public string Content { get; set; }
    }
}