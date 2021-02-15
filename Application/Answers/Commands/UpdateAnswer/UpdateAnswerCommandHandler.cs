using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using QAForum.Application.Common.Interfaces;
using QAForum.Application.Common.Models;
using QAForum.Domain.Entities;

namespace QAForum.Application.Answers.Commands.UpdateAnswer
{
    public class UpdateAnswerCommandHandler : IRequestHandler<UpdateAnswerCommand, Response<object>>
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IAnswerRepository _answerRepository;
        private readonly IAccessValidator _accessValidator;
        private const string UpdateAnswer = "UpdateAnswer";
        private readonly IMapper _mapper;

        public UpdateAnswerCommandHandler(ICurrentUserService currentUserService,
            IAnswerRepository answerRepository,
            IAccessValidator accessValidator,
            IMapper mapper)
        {
            _currentUserService = currentUserService;
            _answerRepository = answerRepository;
            _accessValidator = accessValidator;
            _mapper = mapper;
        }


        public async Task<Response<object>> Handle(UpdateAnswerCommand request, CancellationToken cancellationToken)
        {
            var response = new Response<object>();
            var answer = await _answerRepository.GetByIdAsync(request.Id);
            if (answer is null)
            {
                response.ErrorReason = ErrorReason.NotFound;
                response.Errors = new Dictionary<string, IList<string>>
                {
                    {
                        UpdateAnswer,
                        new List<string> {ErrorMessages.Entity<Answer>.GetNotFoundMessage(request.Id)}
                    }
                };
                return response;
            }

            var userId = _currentUserService.UserId;
            if (!_accessValidator.HasAccessToModify(userId, answer))
            {
                response.ErrorReason = ErrorReason.HaveNoAccess;
                response.Errors = new Dictionary<string, IList<string>>()
                {
                    {
                        UpdateAnswer,
                        new List<string> {ErrorMessages.Entity<Answer>.GetNoAccessMessage(answer.Id, userId)}
                    }
                };
                return response;
            }

            _mapper.Map(request, answer);
            await _answerRepository.UpdateAsync(answer);
            response.SetData();
            return response;
        }
    }
}