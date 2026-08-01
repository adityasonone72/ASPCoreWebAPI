using ASPCoreWebAPI.Responces;

namespace ASPCoreWebAPI.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next; //this is the next middleware, Without it, this middleware cannot continue processing the request.
        private readonly ILogger<ExceptionHandlingMiddleware> _logger; //Ilogger<> is ASP .NET core's built in logging
                                                                       // ReadOnly: For entire lifecycle of middleware object we need fix values of middleware and log. Private: Since we want only this middleware to access it
        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)  //ILogger created by DI, RequestDelegate is created by pipeline builder.
        {
            _next = next;
            _logger = logger;
        }
        public async Task Invoke(HttpContext context) 
        {
            try
            {
                await _next(context); //If an asynchronous operation returns a Task (or Task<T>), and you want to wait for it and observe any exceptions, use await.
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unhandled exception occurred while processing the request."); //ex is whole stack trace, while custom message adds context.

                context.Response.StatusCode= StatusCodes.Status500InternalServerError;
                context.Response.ContentType = "application/JSON";

                var error = new ErrorResponse
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = "An unhandled exception occurred while processing the request.",
                    TraceId = context.TraceIdentifier,
                    TimeStamp = DateTime.UtcNow
                };

                await context.Response.WriteAsJsonAsync(error);
            }
        }
    }
}
