using MediatR;
using QAForum.Application.Common.Models;

namespace QAForum.Application.Users.Commands.LoginCommand
{
    public class LoginUserCommand : IRequest<Response<LoginUserCommandResponse>>
    {
        public string Username { get; set; }

        public string Password { get; set; }
    }
}