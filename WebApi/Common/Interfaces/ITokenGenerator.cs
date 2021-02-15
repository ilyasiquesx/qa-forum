using QAForum.Application.Users.Commands.LoginCommand;

namespace WebApi.Common.Interfaces
{
    public interface ITokenGenerator
    {
        public string GenerateTokenForUser(LoginUserCommandResponse userResponse);
    }
}