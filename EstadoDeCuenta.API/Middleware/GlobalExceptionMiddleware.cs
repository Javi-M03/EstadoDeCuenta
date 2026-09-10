using System.Net;
using System.Text.Json;
using FluentValidation;


namespace EstadoDeCuenta.API.Middleware
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionMiddleware> _logger;

        public GlobalExceptionMiddleware(
            RequestDelegate next,
            ILogger<GlobalExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Se produjo una excepción no controlada.");

                await HandleExceptionAsync(context, ex);
            }
        }

        private static async Task HandleExceptionAsync(
            HttpContext context,
            Exception exception)
        {
            var statusCode = exception switch
            {
                ValidationException => HttpStatusCode.BadRequest,
                KeyNotFoundException => HttpStatusCode.NotFound,
                InvalidOperationException => HttpStatusCode.BadRequest,
                _ => HttpStatusCode.InternalServerError
            };

            var response = new
            {
                statusCode = (int)statusCode,
                message = GetMessage(exception)
            };

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            await context.Response.WriteAsync(
                JsonSerializer.Serialize(response));
        }

        private static string GetMessage(Exception exception)
        {
            if (exception is ValidationException validationException)
            {
                return string.Join(
                    " ",
                    validationException.Errors
                        .Select(error => error.ErrorMessage));
            }

            if (exception is KeyNotFoundException ||
                exception is InvalidOperationException)
            {
                return exception.Message;
            }

            return "Ocurrió un error interno en el servidor.";
        }
    }
}
