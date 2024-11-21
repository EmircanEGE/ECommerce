using Microsoft.AspNetCore.Http;
using System.Text.Json;

namespace ECommerce.Business.Middlewares
{
    public class ValidationMiddleware
    {
        private readonly RequestDelegate _next;

        public ValidationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            if (!context.Response.HasStarted)
            {
                await _next(context);

                if (context.Response.StatusCode == 400 && context.Items.ContainsKey("ValidationErrors"))
                {
                    var errors = (List<string>)context.Items["ValidationErrors"];
                    var response = new { errors };

                    context.Response.ContentType = "application/json";
                    await context.Response.WriteAsync(JsonSerializer.Serialize(response));
                }
            }
        }
    }
}
