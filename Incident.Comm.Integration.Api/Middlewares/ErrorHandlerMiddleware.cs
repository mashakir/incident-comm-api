using Microsoft.AspNetCore.Http;
using Incident.Comm.Integration.Api.Config;
using Incident.Comm.Integration.Api.Exceptions;
using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace Incident.Comm.Integration.Api.Middlewares
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ErrorsSection _errorsSection;

        public ErrorHandlerMiddleware(RequestDelegate next, ErrorsSection errorsSection)
        {
            _next = next;
            _errorsSection = errorsSection;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception error)
            {
                var response = context.Response;
                response.ContentType = "application/json";

                response.StatusCode = error switch
                {
                    AppException => (int)HttpStatusCode.InternalServerError,
                    NotFoundException => (int)HttpStatusCode.NotFound,
                    BadRequestException => (int)HttpStatusCode.BadRequest,
                    _ => (int)HttpStatusCode.InternalServerError,
                };

                string message;
                if (_errorsSection.ReturnErrorDetails)
                {
                    message = error?.Message;
                }
                else
                {
                    message = error.GetType().ToString();
                }

                var result = JsonSerializer.Serialize(new { message = message });
                await response.WriteAsync(result);
            }
        }
    }
}
