using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using QAForum.Application.Common.Interfaces;
using QAForum.Application.Common.Models;
using QAForum.Domain.Entities;

namespace QAForum.Application.Questions.Commands.CreateQuestion
{
    public class
        CreateQuestionCommandHandler : IRequestHandler<CreateQuestionCommand, Response<CreateQuestionCommandResponse>>
    {
        private readonly IMapper _mapper;
        private readonly IQuestionRepository _questionRepository;

        public CreateQuestionCommandHandler(IMapper mapper,
            IQuestionRepository questionRepository)
        {
            _mapper = mapper;
            _questionRepository = questionRepository;
        }

        public async Task<Response<CreateQuestionCommandResponse>> Handle(CreateQuestionCommand request,
            CancellationToken cancellationToken)
        {
            var response = new Response<CreateQuestionCommandResponse>();
            var question = _mapper.Map<Question>(request);
            await _questionRepository.CreateAsync(question);
            var data = new CreateQuestionCommandResponse
            {
                Id = question.Id
            };
            response.SetData(data);
            return response;
        }
    }
}