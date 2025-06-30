using System.Text.Json;
using Cloud.Domain.Exceptions;

namespace Cloud.Middleware;

public class ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
{
    public async Task Invoke(HttpContext context)
    {

        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            
            context.Response.ContentType = "application/json";

            context.Response.StatusCode = ex switch
            {
                NotFoundException => StatusCodes.Status404NotFound,
                ForbiddenException => StatusCodes.Status403Forbidden,
                ConflictException => StatusCodes.Status409Conflict,
                _ => StatusCodes.Status500InternalServerError
            };

            var response = new
            {
                error = ex.Message,
                status = context.Response.StatusCode,
            };
            
            var json = JsonSerializer.Serialize(response);
            await context.Response.WriteAsync(json);

        }

    }

}