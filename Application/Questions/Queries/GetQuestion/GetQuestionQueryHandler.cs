using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using QAForum.Application.Common.Interfaces;
using QAForum.Application.Common.Models;
using QAForum.Domain.Entities;

namespace QAForum.Application.Questions.Queries.GetQuestion
{
    public class GetQuestionQueryHandler : IRequestHandler<GetQuestionQuery, Response<GetQuestionQueryResponse>>
    {
        private readonly IQuestionRepository _questionRepository;
        private readonly IMapper _mapper;
        private const string GetQuestionKey = "GetQuestion";

        public GetQuestionQueryHandler(IQuestionRepository questionRepository,
            IMapper mapper)
        {
            _questionRepository = questionRepository;
            _mapper = mapper;
        }

        public async Task<Response<GetQuestionQueryResponse>> Handle(GetQuestionQuery request,
            CancellationToken cancellationToken)
        {
            var response = new Response<GetQuestionQueryResponse>();
            var question = await _questionRepository.GetByIdAsync(request.Id);
            if (question is null)
            {
                response.ErrorReason = ErrorReason.NotFound;
                response.Errors = new Dictionary<string, IList<string>>
                {
                    {GetQuestionKey, new List<string> {ErrorMessages.Entity<Question>.GetNotFoundMessage(request.Id)}}
                };
                return response;
            }

            var data = _mapper.Map<GetQuestionQueryResponse>(question);
            if (data.BestAnswer is not null)
            {
                var itemToRemove = data.Answers.First(a => a.Id == data.BestAnswer.Id);
                data.Answers.Remove(itemToRemove);
            }

            data.Answers = data.Answers.OrderBy(a => a.CreatedAt).ToList();
            response.SetData(data);
            return response;
        }
    }
}