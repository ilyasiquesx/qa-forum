using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using QAForum.Application.Common.Interfaces;
using QAForum.Application.Common.Models;
using QAForum.Domain.Entities;

namespace QAForum.Application.Answers.Commands.CreateAnswer
{
    public class
        CreateAnswerCommandHandler : IRequestHandler<CreateAnswerCommand, Response<CreateAnswerCommandResponse>>
    {
        private readonly IAnswerRepository _answerRepository;
        private readonly IQuestionRepository _questionRepository;
        private const string CreateAnswerKey = "CreateAnswer";
        private readonly IMapper _mapper;

        public CreateAnswerCommandHandler(IAnswerRepository answerRepository,
            IQuestionRepository questionRepository, IMapper mapper)
        {
            _answerRepository = answerRepository;
            _questionRepository = questionRepository;
            _mapper = mapper;
        }

        public async Task<Response<CreateAnswerCommandResponse>> Handle(CreateAnswerCommand request,
            CancellationToken cancellationToken)
        {
            var response = new Response<CreateAnswerCommandResponse>();
            if (!await _questionRepository.DoesExistAsync(request.QuestionId))
            {
                response.ErrorReason = ErrorReason.NotFound;
                response.Errors.Add(CreateAnswerKey,
                    new List<string> {ErrorMessages.Entity<Question>.GetNotFoundMessage(request.QuestionId)});
                return response;
            }

            var answer = _mapper.Map<Answer>(request);
            await _answerRepository.CreateAsync(answer);
            var data = new CreateAnswerCommandResponse
            {
                QuestionId = answer.QuestionId
            };
            response.SetData(data);
            return response;
        }
    }
}