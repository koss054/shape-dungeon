using Microsoft.AspNetCore.Mvc;
using ShapeDungeon.Exceptions;
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
                if (e is NoActiveCombatException combatEx)
                {
                    context.Response.StatusCode = combatEx.StatusCode;
                    context.Response.Redirect($"/Home/Error?statusCode={combatEx.StatusCode}");
                } 
                else
                {
                    context.Response.Redirect("/Home/Error");
                }
            }
        }
    }
}
