using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace QuizWhois.Common
{
    public class CustomExceptionFilter : IExceptionFilter
    {
        private readonly ILogger _logger;

        public CustomExceptionFilter(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<CustomExceptionFilter>();
        }

        public void OnException(ExceptionContext filterContext)
        {
            _logger.LogError($"{filterContext.Exception.Message}\n{filterContext.Exception.StackTrace}");
        }
    }
}
