using MediatR;
using QAForum.Application.Common.Models;

namespace QAForum.Application.Questions.Commands.DeleteQuestion
{
    public class DeleteQuestionCommand : IRequest<Response<object>>
    {
        public long Id { get; set; }
    }
}