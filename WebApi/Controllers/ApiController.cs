using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using QAForum.Application.Common.Models;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ApiController : ControllerBase
    {
        protected readonly IMediator Mediator;
        protected readonly ILogger<ApiController> Logger;
        private const string LoggingMessageTemplate = "{Message} {ResponseType} {Response}";
        private const string LoggingMessageOnSuccess = "Successful action execution";
        private const string LoggingMessageOnError = "Failed at execution";

        public ApiController(IMediator mediator, ILogger<ApiController> logger)
        {
            Mediator = mediator;
            Logger = logger;
        }

        protected IActionResult GetResponse<T>(Response<T> response)
        {
            var jsonResponse = JsonConvert.SerializeObject(response);
            var responseTypeName = typeof(T).Name;
            if (response.IsSucceeded)
            {
                Logger.LogInformation(LoggingMessageTemplate, LoggingMessageOnSuccess, responseTypeName, jsonResponse);
                if (response.Data is null)
                {
                    return NoContent();
                }

                return Ok(response.Data);
            }

            Logger.LogWarning(LoggingMessageTemplate, LoggingMessageOnError, responseTypeName, jsonResponse);
            var errors = response.Errors;
            return response.ErrorReason switch
            {
                ErrorReason.InvalidData => StatusCode(StatusCodes.Status400BadRequest, new {errors}),
                ErrorReason.WrongCredentials => StatusCode(StatusCodes.Status401Unauthorized, new {errors}),
                ErrorReason.HaveNoAccess => StatusCode(StatusCodes.Status403Forbidden, new {errors}),
                ErrorReason.NotFound => StatusCode(StatusCodes.Status404NotFound, new {errors}),
                ErrorReason.AlreadyExist => StatusCode(StatusCodes.Status409Conflict, new {errors}),
                _ => StatusCode(StatusCodes.Status400BadRequest)
            };
        }
    }
}