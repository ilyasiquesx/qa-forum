using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace WebApi.Common.Filters
{
    public class ExceptionFilter : ExceptionFilterAttribute
    {
        private readonly ILogger<ExceptionFilter> _logger;

        public ExceptionFilter(ILogger<ExceptionFilter> logger)
        {
            _logger = logger;
        }

        public override void OnException(ExceptionContext context)
        {
            _logger.LogError(context.Exception, "Got unhandled exception");
            var traceIdentifier = context.HttpContext.TraceIdentifier;
            context.Result = new ObjectResult(new {traceIdentifier})
            {
                StatusCode = StatusCodes.Status500InternalServerError
            };

            base.OnException(context);
        }
    }
}