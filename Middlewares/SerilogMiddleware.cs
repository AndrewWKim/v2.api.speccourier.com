using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Serilog.Context;
using SpeccourierApiV2.Extensions;

namespace SpeccourierApiV2.Middlewares
{
	public class SerilogMiddleware
	{
		private const string RemoteIpPropertyName = "RemoteIp";
		private const string RequestPathPropertyName = "RequestPath";
		private const string UsernamePropertyName = "Username";
		private const string RequestIdPropertyName = "RequestsId";
		private const string OptionsRequestMethod = "OPTIONS";
		private readonly ILogger<SerilogMiddleware> _logger;

		private readonly RequestDelegate _next;

		public SerilogMiddleware(
			RequestDelegate next,
			ILogger<SerilogMiddleware> logger)
		{
			_next = next;
			_logger = logger;
		}

		public async Task Invoke(HttpContext context)
		{
			using (LogContext.PushProperty(RemoteIpPropertyName, context.Connection.RemoteIpAddress))
			{
				using (LogContext.PushProperty(RequestPathPropertyName, context.Request.Path))
				{
					using (LogContext.PushProperty(RequestIdPropertyName, Guid.NewGuid().ToString()))
					{
						if (context.Request.Method != OptionsRequestMethod)
						{
							_logger.LogInformation(await context.GetRequestBodyString());

							var originalBodyStream = context.Response.Body;

							using (var responseBody = new MemoryStream())
							{
								context.Response.Body = responseBody;

								await _next.Invoke(context);

								context.Response.Body.Seek(0, SeekOrigin.Begin);

								string text = await new StreamReader(context.Response.Body).ReadToEndAsync();

								context.Response.Body.Seek(0, SeekOrigin.Begin);

								_logger.LogInformation($"{context.Response.StatusCode}: {text}");

								await responseBody.CopyToAsync(originalBodyStream);
							}
						}
						else
						{
							await _next.Invoke(context);
						}
					}
				}
			}
		}
	}
}
