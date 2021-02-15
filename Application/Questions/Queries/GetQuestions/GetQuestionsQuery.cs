using MediatR;
using QAForum.Application.Common.Models;

namespace QAForum.Application.Questions.Queries.GetQuestions
{
    public class GetQuestionsQuery : IRequest<Response<GetQuestionsQueryResponse>>
    {
        private const int DefaultPageSize = 20;
        private const int DefaultPage = 1;
        private readonly int _page;
        private readonly int _pageSize;

        public int Page
        {
            get => _page;
            init => _page = value < 1 ? DefaultPage : value;
        }

        public int PageSize
        {
            get => _pageSize;
            init => _pageSize = value < 1 ? DefaultPageSize : value;
        }
    }
}