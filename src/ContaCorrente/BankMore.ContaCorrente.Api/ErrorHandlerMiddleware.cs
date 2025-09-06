using Microsoft.AspNetCore.Http;
using System.Net;
using System.Text.Json;

namespace BankMore.ContaCorrente.Api.Middlewares
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            var response = exception switch
            {
                UnauthorizedAccessException => new { message = exception.Message, type = "USER_UNAUTHORIZED" },
                InvalidOperationException ioe => new
                {
                    message = ioe.Message,
                    type = ioe.Message switch
                    {
                        "Conta não cadastrada." => "INVALID_ACCOUNT",
                        "Conta inativa." => "INACTIVE_ACCOUNT",
                        "Valor deve ser positivo." => "INVALID_VALUE",
                        "Tipo de movimento inválido." => "INVALID_TYPE",
                        "Transferência só permite crédito na conta de destino." => "INVALID_TYPE",
                        "CPF inválido." => "INVALID_DOCUMENT",
                        _ => "ERROR"
                    }
                },
                _ => new { message = exception.Message, type = "ERROR" }
            };

            context.Response.StatusCode = exception switch
            {
                UnauthorizedAccessException => (int)HttpStatusCode.Unauthorized,
                InvalidOperationException => (int)HttpStatusCode.BadRequest,
                _ => (int)HttpStatusCode.InternalServerError
            };

            var json = JsonSerializer.Serialize(response);
            return context.Response.WriteAsync(json);
        }
    }
}
