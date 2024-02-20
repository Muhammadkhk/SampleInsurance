using Microsoft.AspNetCore.Mvc;

namespace Sample.API
{
    public static class RequestHandler
    {


        public static async Task<IActionResult> HandleCommand<T, TResult>(
            T request, Func<T, Task<TResult>> handler)
        {
            try
            {
                //log.Debug("Handling HTTP request of type {type}", typeof(T).Name);
                TResult result = await handler(request);
                return new OkObjectResult(result);
            }
            catch (UnauthorizedAccessException e)
            {
                //log.Error(e, "Error handling the command");
                return new UnauthorizedObjectResult(new
                {
                    error = e.Message,
                    stackTrace = e.StackTrace
                });
            }
            catch (Exception e)
            {
                //  log.Error(e, "Error handling the command");
                return new BadRequestObjectResult(new
                {
                    error = e.Message,
                    stackTrace = e.StackTrace
                });
            }
        }

      
        public static async Task<IActionResult> HandleQuery<TResult>(
         Func<Task<TResult>> handler)
        {
            try
            {
                TResult result = await handler();
                return new OkObjectResult(result);
            }
            catch (Exception e)
            {
                // log.Error(e, "Error handling the query");
                return new BadRequestObjectResult(new
                {
                    error = e.Message,
                    stackTrace = e.StackTrace
                });
            }
        }

        public static async Task<IActionResult> HandleQuery<T, TResult>(
            T request, Func<T, Task<TResult>> handler)
        {
            try
            {
                TResult result = await handler(request);
                return new OkObjectResult(result);
            }
            catch (Exception e)
            {
                //  log.Error(e, "Error handling the query");
                return new BadRequestObjectResult(new
                {
                    error = e.Message,
                    stackTrace = e.StackTrace
                });
            }
        }
    }
}