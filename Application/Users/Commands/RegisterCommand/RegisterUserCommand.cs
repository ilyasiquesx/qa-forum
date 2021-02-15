using MediatR;
using QAForum.Application.Common;
using QAForum.Application.Common.Models;

namespace QAForum.Application.Users.Commands.RegisterCommand
{
    public class RegisterUserCommand : IRequest<Response<RegisterUserCommandResponse>>
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
}