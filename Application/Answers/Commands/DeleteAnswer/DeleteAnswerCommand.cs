using MediatR;
using QAForum.Application.Common.Models;

namespace QAForum.Application.Answers.Commands.DeleteAnswer
{
    public class DeleteAnswerCommand : IRequest<Response<object>>
    {
        public long Id { get; set; }
    }
}