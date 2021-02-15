using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using QAForum.Application.Common.Interfaces;
using QAForum.Application.Common.Models;
using QAForum.Domain.Entities;

namespace QAForum.Application.Questions.Commands.UpdateQuestion
{
    public class UpdateQuestionCommandHandler : IRequestHandler<UpdateQuestionCommand, Response<object>>
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IQuestionRepository _questionRepository;
        private readonly IAccessValidator _accessValidator;
        private readonly IMapper _mapper;
        private readonly IAnswerRepository _answerRepository;
        private const string UpdateQuestionKey = "UpdateQuestion";

        public UpdateQuestionCommandHandler(ICurrentUserService currentUserService,
            IQuestionRepository questionRepository,
            IMapper mapper,
            IAccessValidator accessValidator, IAnswerRepository answerRepository)
        {
            _currentUserService = currentUserService;
            _questionRepository = questionRepository;
            _mapper = mapper;
            _accessValidator = accessValidator;
            _answerRepository = answerRepository;
        }

        public async Task<Response<object>> Handle(UpdateQuestionCommand request, CancellationToken cancellationToken)
        {
            var response = new Response<object>();
            var question = await _questionRepository.GetByIdAsync(request.Id);
            if (question is null)
            {
                response.ErrorReason = ErrorReason.NotFound;
                response.Errors = new Dictionary<string, IList<string>>
                {
                    {
                        UpdateQuestionKey,
                        new List<string> {ErrorMessages.Entity<Question>.GetNotFoundMessage(request.Id)}
                    }
                };
                return response;
            }

            if (request.BestAnswerId.HasValue &&
                !await _answerRepository.DoesAnswerExistAsync(request.BestAnswerId.Value))
            {
                response.ErrorReason = ErrorReason.NotFound;
                response.Errors = new Dictionary<string, IList<string>>
                {
                    {
                        UpdateQuestionKey,
                        new List<string> {ErrorMessages.Entity<Answer>.GetNotFoundMessage(request.BestAnswerId.Value)}
                    }
                };
                return response;
            }

            var userId = _currentUserService.UserId;
            if (!_accessValidator.HasAccessToModify(userId, question))
            {
                response.ErrorReason = ErrorReason.HaveNoAccess;
                response.Errors = new Dictionary<string, IList<string>>()
                {
                    {
                        UpdateQuestionKey,
                        new List<string> {ErrorMessages.Entity<Question>.GetNoAccessMessage(question.Id, userId)}
                    }
                };
                return response;
            }

            _mapper.Map(request, question);
            await _questionRepository.UpdateAsync(question);
            response.SetData();
            return response;
        }
    }
}