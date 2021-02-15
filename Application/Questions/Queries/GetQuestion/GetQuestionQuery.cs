using MediatR;
using QAForum.Application.Common.Models;

namespace QAForum.Application.Questions.Queries.GetQuestion
{
    public class GetQuestionQuery : IRequest<Response<GetQuestionQueryResponse>>
    {
        public long Id { get; set; }
    }
}