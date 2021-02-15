using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using QAForum.Application.Common.Interfaces;
using QAForum.Application.Common.Models;
using QAForum.Domain.Entities;

namespace QAForum.Application.Questions.Commands.DeleteQuestion
{
    public class DeleteQuestionCommandHandler : IRequestHandler<DeleteQuestionCommand, Response<object>>
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IQuestionRepository _questionRepository;
        private readonly IAccessValidator _accessValidator;
        private const string DeleteQuestionKey = "DeleteQuestion";

        public DeleteQuestionCommandHandler(ICurrentUserService currentUserService,
            IQuestionRepository questionRepository,
            IAccessValidator accessValidator)
        {
            _currentUserService = currentUserService;
            _questionRepository = questionRepository;
            _accessValidator = accessValidator;
        }

        public async Task<Response<object>> Handle(DeleteQuestionCommand request, CancellationToken cancellationToken)
        {
            var response = new Response<object>();
            var question = await _questionRepository.GetByIdAsync(request.Id);
            if (question is null)
            {
                response.ErrorReason = ErrorReason.NotFound;
                response.Errors = new Dictionary<string, IList<string>>
                {
                    {
                        DeleteQuestionKey,
                        new List<string> {ErrorMessages.Entity<Question>.GetNotFoundMessage(request.Id)}
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
                        DeleteQuestionKey,
                        new List<string> {ErrorMessages.Entity<Question>.GetNoAccessMessage(question.Id, userId)}
                    }
                };
                return response;
            }

            await _questionRepository.DeleteAsync(question);
            response.SetData();
            return response;
        }
    }
}