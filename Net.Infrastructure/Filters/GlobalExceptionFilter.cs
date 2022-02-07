using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Net.Infrastructure.ErrorsHandling;
using Net.Infrastructure.Exceptions;
using Net.Infrastructure.Extensions;

namespace Net.Infrastructure.Filters
{
    public class GlobalExceptionFilter : IExceptionFilter
    {
        private readonly ILogger _logger;
        public GlobalExceptionFilter(ILogger<GlobalExceptionFilter> logger)
        {
            _logger = logger;
        }
        public void OnException(ExceptionContext context)
        {
            var exception = context.Exception;
            var error = ErrorResponseBuilder.Create(exception);
            var responseText = new {error }.ToJson();
            var apiException = new ApiException(error.ToJson());

            _logger.LogError(apiException, "");
            
            context.Result = new ContentResult {
                Content = responseText,
                ContentType = "application/json",
                StatusCode = (int)error.StatusCode
            };

            context.ExceptionHandled = true;
        }
    }
}