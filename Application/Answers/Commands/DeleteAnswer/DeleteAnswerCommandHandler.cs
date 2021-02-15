using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using QAForum.Application.Common.Interfaces;
using QAForum.Application.Common.Models;
using QAForum.Domain.Entities;

namespace QAForum.Application.Answers.Commands.DeleteAnswer
{
    public class DeleteAnswerCommandHandler : IRequestHandler<DeleteAnswerCommand, Response<object>>
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IAnswerRepository _answerRepository;
        private readonly IAccessValidator _accessValidator;
        private const string DeleteAnswer = "DeleteAnswer";

        public DeleteAnswerCommandHandler(ICurrentUserService currentUserService,
            IAnswerRepository answerRepository,
            IAccessValidator accessValidator)
        {
            _currentUserService = currentUserService;
            _answerRepository = answerRepository;
            _accessValidator = accessValidator;
        }

        public async Task<Response<object>> Handle(DeleteAnswerCommand request, CancellationToken cancellationToken)
        {
            var response = new Response<object>();
            var answer = await _answerRepository.GetByIdAsync(request.Id);
            if (answer is null)
            {
                response.ErrorReason = ErrorReason.NotFound;
                response.Errors = new Dictionary<string, IList<string>>
                {
                    {
                        DeleteAnswer,
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
                        DeleteAnswer,
                        new List<string> {ErrorMessages.Entity<Answer>.GetNoAccessMessage(answer.Id, userId)}
                    }
                };
                return response;
            }

            await _answerRepository.DeleteAsync(answer);
            response.SetData();
            return response;
        }
    }
}