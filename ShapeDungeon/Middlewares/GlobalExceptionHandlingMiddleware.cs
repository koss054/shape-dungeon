using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;

namespace ShapeDungeon.Middlewares
{
    public class GlobalExceptionHandlingMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception e)
            {
                context.Response.StatusCode =
                    (int)HttpStatusCode.InternalServerError;

                var problemDetails = new ProblemDetails()
                {
                    Status = (int)HttpStatusCode.InternalServerError,
                    Type = "Server error",
                    Title = "Server error",
                    Detail = e.Message
                };

                string json = JsonSerializer.Serialize(problemDetails);

                await context.Response.WriteAsync(json);
                context.Response.ContentType = "application/json";
            }
        }
    }
}
