using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using QAForum.Application.Answers.Commands.CreateAnswer;
using QAForum.Application.Answers.Commands.DeleteAnswer;
using QAForum.Application.Answers.Commands.UpdateAnswer;

namespace WebApi.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class AnswerController : ApiController
    {
        public AnswerController(IMediator mediator, ILogger<ApiController> logger) : base(mediator, logger)
        {
        }

        /// <summary>
        /// Метод создания ответа
        /// </summary>
        /// <param name="command">Тело запроса</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Create(CreateAnswerCommand command)
        {
            var result = await Mediator.Send(command);
            return GetResponse(result);
        }

        /// <summary>
        /// Метод удаления ответа
        /// </summary>
        /// <param name="id">Идентификатор ответа</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            var result = await Mediator.Send(new DeleteAnswerCommand {Id = id});
            return GetResponse(result);
        }

        /// <summary>
        /// Метод обновления ответа
        /// </summary>
        /// <param name="command">Тело запроса</param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> Update(UpdateAnswerCommand command)
        {
            var result = await Mediator.Send(command);
            return GetResponse(result);
        }
    }
}