using MediatR;
using QAForum.Application.Common.Models;

namespace QAForum.Application.Answers.Commands.UpdateAnswer
{
    public class UpdateAnswerCommand : IRequest<Response<object>>
    {
        public long Id { get; set; }
        public string Content { get; set; }
    }
}