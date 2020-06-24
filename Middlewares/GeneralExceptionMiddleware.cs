using Microsoft.AspNetCore.Http;
using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace SpeccourierApiV2.Middlewares
{
    public class GeneralExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GeneralExceptionMiddleware> _logger;

        public GeneralExceptionMiddleware(RequestDelegate next, ILogger<GeneralExceptionMiddleware> logger)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (ArgumentNullException ex)
            {
                await HandleException(context.Response, LogLevel.Error, ex, HttpStatusCode.NotFound);
            }
            catch (Exception ex)
            {
                await HandleException(context.Response, LogLevel.Error, ex, HttpStatusCode.InternalServerError);
            }
        }

        private Task HandleException(HttpResponse response, LogLevel loggLevel, Exception exception, HttpStatusCode statusCode, object responseToWrite = null)
        {
            _logger.Log(loggLevel, exception, exception.Message);
            response.StatusCode = (int)statusCode;

            return responseToWrite != null ?
                response.WriteAsync(
                    JsonConvert.SerializeObject(
                        responseToWrite,
                        Formatting.None,
                        new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() }))
                : Task.CompletedTask;
        }
    }
}
