

using System.Net;

namespace TripAssistant.Api.Middlewares
{
    public class ExceptionMiddleware(RequestDelegate next,
      ILogger<ExceptionMiddleware> logger,
      IHostEnvironment env)
    {
        public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "{0},{1}", ex.Message, ex.StackTrace);
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            var Response = env.IsDevelopment() ?
                          new ApiExceptionDto(context.Response.StatusCode,
                                            ex.Message) :
                          new ApiExceptionDto(context.Response.StatusCode,
                                            "Internal Server Error");
            var option = new System.Text.Json.JsonSerializerOptions
            {
                PropertyNamingPolicy = System.Text.Json
                                    .JsonNamingPolicy
                                    .CamelCase
            };
            var json = System.Text.Json
                      .JsonSerializer
                      .Serialize(Response, option);
            await context.Response.WriteAsync(json);

        }
    }
}
public class ApiExceptionDto(int statusCode, string message)
    {
        public int StatusCode { get; set; } = statusCode;
public string Message { get; set; } = message;
    }
}
