using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using QAForum.Application.Users.Commands.LoginCommand;
using QAForum.Application.Users.Commands.RegisterCommand;
using WebApi.Common.Interfaces;
using WebApi.Models;

namespace WebApi.Controllers
{
    public class AccountController : ApiController
    {
        private readonly ITokenGenerator _tokenGenerator;

        public AccountController(IMediator mediator,
            ILogger<AccountController> logger,
            ITokenGenerator tokenGenerator) : base(mediator, logger)
        {
            _tokenGenerator = tokenGenerator;
        }

        /// <summary>
        /// Метод регистрации пользователя
        /// </summary>
        /// <param name="command">Тело запроса</param>
        /// <returns></returns>
        [HttpPost(nameof(Register))]
        public async Task<IActionResult> Register([FromBody] RegisterUserCommand command)
        {
            Logger.LogInformation("{Message} {Username}",
                "Trying to register user", command.Username);

            var result = await Mediator.Send(command);
            if (!result.IsSucceeded)
            {
                Logger.LogWarning("{Message} {ErrorMessage} {Username} ",
                    "Failed at register user", result.Errors.Values, command.Username);
                return GetResponse(result);
            }

            var loginCommand = new LoginUserCommand
            {
                Username = command.Username,
                Password = command.Password
            };

            Logger.LogInformation("{Message} {Username}", "Successful register for user. Using login method",
                command.Username);
            return await Login(loginCommand);
        }

        /// <summary>
        /// Метод авторизации пользователя
        /// </summary>
        /// <param name="command">Тело запроса</param>
        /// <returns></returns>
        [HttpPost(nameof(Login))]
        public async Task<IActionResult> Login([FromBody] LoginUserCommand command)
        {
            Logger.LogInformation("{Message} {Username}", "Trying to login user", command.Username);
            var result = await Mediator.Send(command);
            if (!result.IsSucceeded)
            {
                Logger.LogWarning("{Message} {ErrorMessage} {Username} ",
                    "Failed at login user", result.Errors.Values, command.Username);
                return GetResponse(result);
            }

            var userData = result.Data;
            var response = new ApiLoginResponse
            {
                Id = userData.Id,
                Username = userData.Username,
                Email = userData.Email,
                Token = _tokenGenerator.GenerateTokenForUser(userData)
            };
            Logger.LogInformation("{Message} {UserId} {Username}",
                "Successful login for user", userData.Id, userData.Username);
            return Ok(response);
        }
    }
}