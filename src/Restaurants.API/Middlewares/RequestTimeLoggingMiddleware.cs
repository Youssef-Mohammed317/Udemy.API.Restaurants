using System.Diagnostics;

namespace Restaurants.API.Middlewares;

public class RequestTimeLoggingMiddleware(ILogger<RequestTimeLoggingMiddleware> _logger) : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var sw = Stopwatch.StartNew();
        try
        {
            await next(context);
        }
        finally
        {
            sw.Stop();

            if (sw.ElapsedMilliseconds > 4000) // more than 4 seconds
            {
                var verb = context.Request.Method;
                var path = context.Request.Path;

                _logger.LogInformation(
                   "Request [{Verb} at {Path}] took {Time} ms",
                   verb,
                   path,
                   sw.ElapsedMilliseconds
               );
            }
        }
    }
}
