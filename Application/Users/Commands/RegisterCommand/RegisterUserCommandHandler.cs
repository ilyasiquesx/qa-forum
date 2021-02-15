using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using QAForum.Application.Common.Interfaces;
using QAForum.Application.Common.Models;
using QAForum.Domain.Entities;

namespace QAForum.Application.Users.Commands.RegisterCommand
{
    public class
        RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, Response<RegisterUserCommandResponse>>
    {
        private const string RegisterKey = "Register";
        private readonly IIdentityService _identityService;
        private readonly IMapper _mapper;

        public RegisterUserCommandHandler(IIdentityService identityService,
            IMapper mapper)
        {
            _identityService = identityService;
            _mapper = mapper;
        }

        public async Task<Response<RegisterUserCommandResponse>> Handle(RegisterUserCommand request,
            CancellationToken cancellationToken)
        {
            var response = new Response<RegisterUserCommandResponse>();
            var user = await _identityService.GetUserByUsernameAsync(request.Username);
            if (user is not null)
            {
                response.Errors.Add(RegisterKey, new List<string> {ErrorMessages.Account.GetUserExistMessage});
                response.ErrorReason = ErrorReason.AlreadyExist;
                return response;
            }

            user = _mapper.Map<User>(request);
            var result = await _identityService.CreateUserAsync(user, request.Password);
            if (!result.IsSucceeded)
            {
                response.Errors.Add(RegisterKey, result.Errors.ToList());
                response.ErrorReason = ErrorReason.InvalidData;
                return response;
            }

            var data = new RegisterUserCommandResponse
            {
                Id = user.Id
            };
            response.SetData(data);
            return response;
        }
    }
}