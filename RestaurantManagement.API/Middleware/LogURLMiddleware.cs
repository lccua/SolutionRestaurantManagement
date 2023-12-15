using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Serilog;
using System.Threading.Tasks;
namespace RestaurantManagement.API.Middleware
{
    public class LogURLMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly Microsoft.Extensions.Logging.ILogger _logger;

        public LogURLMiddleware(RequestDelegate next, ILoggerFactory loggerFactory)
        {
            _next = next;
            _logger = loggerFactory.CreateLogger<LogURLMiddleware>();
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            finally
            {
                var logMessage = $"Request Method: {context.Request.Method} | Request Path: {context.Request.Path} => Status: {context.Response.StatusCode}";

                _logger.LogInformation(logMessage);

            }
        }
    }
}
