using Microsoft.AspNetCore.Mvc;

namespace HrPortal.Api.Infrastructure;

public sealed class ExceptionHandlingMiddleware : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (ApiException ex)
        {
            await WriteProblemAsync(context, ex.StatusCode, ex.Message);
        }
        catch (Exception)
        {
            await WriteProblemAsync(context, StatusCodes.Status500InternalServerError, "An unexpected error occurred.");
        }
    }

    private static Task WriteProblemAsync(HttpContext context, int statusCode, string message)
    {
        context.Response.StatusCode = statusCode;
        context.Response.ContentType = "application/problem+json";

        var problem = new ProblemDetails
        {
            Status = statusCode,
            Title = message
        };

        return context.Response.WriteAsJsonAsync(problem);
    }
}
