using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using QAForum.Application.Questions.Commands.CreateQuestion;
using QAForum.Application.Questions.Commands.DeleteQuestion;
using QAForum.Application.Questions.Commands.UpdateQuestion;
using QAForum.Application.Questions.Queries.GetQuestion;
using QAForum.Application.Questions.Queries.GetQuestions;

namespace WebApi.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class QuestionController : ApiController
    {
        public QuestionController(IMediator mediator, ILogger<QuestionController> logger) : base(mediator, logger)
        {
        }

        /// <summary>
        /// Метод открытия нового вопроса
        /// </summary>
        /// <param name="command">Тело запроса</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Create(CreateQuestionCommand command)
        {
            var result = await Mediator.Send(command);
            return GetResponse(result);
        }

        /// <summary>
        /// Метод обновления вопроса
        /// </summary>
        /// <param name="command">Тело запроса</param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> Update(UpdateQuestionCommand command)
        {
            var result = await Mediator.Send(command);
            return GetResponse(result);
        }

        /// <summary>
        /// Метод удаления вопроса
        /// </summary>
        /// <param name="id">Идентификатор вопроса</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            var result = await Mediator.Send(new DeleteQuestionCommand {Id = id});
            return GetResponse(result);
        }

        /// <summary>
        /// Метод получения подробностей вопроса
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(long id)
        {
            var result = await Mediator.Send(new GetQuestionQuery {Id = id});
            return GetResponse(result);
        }

        /// <summary>
        /// Метод получения списка вопросов
        /// </summary>
        /// <param name="page">Запрашиваемая страница</param>
        /// <param name="pageSize">Количество элементов на странице</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] int page, [FromQuery] int pageSize)
        {
            var result = await Mediator.Send(new GetQuestionsQuery {Page = page, PageSize = pageSize});
            return GetResponse(result);
        }
    }
}