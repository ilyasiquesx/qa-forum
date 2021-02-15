using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using QAForum.Application.Common.Interfaces;
using QAForum.Application.Common.Models;

namespace QAForum.Application.Users.Commands.LoginCommand
{
    public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, Response<LoginUserCommandResponse>>
    {
        private readonly IIdentityService _identityService;
        private const string LoginKey = "Login";

        public LoginUserCommandHandler(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public async Task<Response<LoginUserCommandResponse>> Handle(LoginUserCommand request,
            CancellationToken cancellationToken)
        {
            var response = new Response<LoginUserCommandResponse>();
            var user = await _identityService.GetUserByUsernameAsync(request.Username);
            if (user is null)
            {
                response.ErrorReason = ErrorReason.NotFound;
                response.Errors.Add(LoginKey, new List<string> {ErrorMessages.Account.GetUserNotFoundMessage});
                return response;
            }

            var isPassValid = await _identityService.CheckPasswordAsync(user, request.Password);
            if (isPassValid)
            {
                var data = new LoginUserCommandResponse
                {
                    Id = user.Id,
                    Username = user.UserName,
                    Email = user.Email
                };
                response.SetData(data);
                return response;
            }

            response.ErrorReason = ErrorReason.WrongCredentials;
            response.Errors.Add(LoginKey, new List<string> {ErrorMessages.Account.GetUserWrongInfoMessage});
            return response;
        }
    }
}