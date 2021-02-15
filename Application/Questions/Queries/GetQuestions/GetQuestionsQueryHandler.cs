using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using QAForum.Application.Common.Extensions;
using QAForum.Application.Common.Interfaces;
using QAForum.Application.Common.Models;
using QAForum.Application.Questions.Models;

namespace QAForum.Application.Questions.Queries.GetQuestions
{
    public class GetQuestionsQueryHandler : IRequestHandler<GetQuestionsQuery, Response<GetQuestionsQueryResponse>>
    {
        private readonly IQuestionRepository _questionRepository;
        private readonly IMapper _mapper;

        public GetQuestionsQueryHandler(IAnswerRepository answerRepository, IQuestionRepository questionRepository,
            IMapper mapper)
        {
            _questionRepository = questionRepository;
            _mapper = mapper;
        }

        public Task<Response<GetQuestionsQueryResponse>> Handle(GetQuestionsQuery request,
            CancellationToken cancellationToken)
        {
            var response = new Response<GetQuestionsQueryResponse>();
            var questions = _questionRepository.GetAll().OrderByDescending(a => a.UpdatedAt);
            var (paginatedCollection, page, pages) = questions.PaginateCollection(request.PageSize, request.Page);
            var questionsResult = _mapper.Map<IEnumerable<QuestionIndexModel>>(paginatedCollection);
            var data = new GetQuestionsQueryResponse
            {
                Pages = pages,
                Page = page,
                Questions = questionsResult
            };
            response.SetData(data);
            return Task.FromResult(response);
        }
    }
}