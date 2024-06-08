#region Usings

using System.Net;
using System.Text.Json;

#endregion

namespace NewmanRingsTwice.API.Setup
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch
            {
                var result = new ErrorReponse();

                context.Response.ContentType = result.ContentType;
                context.Response.StatusCode = (int)result.Code;

                await context.Response.WriteAsync(JsonSerializer.Serialize(result));
            }
        }

        private class ErrorReponse
        {
            public string Title => "Error";
            public string Message => "Error";
            public HttpStatusCode Code => HttpStatusCode.InternalServerError;
            public string ContentType { get; } = "application/json";
        }
    }
}
