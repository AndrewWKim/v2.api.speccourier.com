using Microsoft.AspNetCore.Builder;
using SpeccourierApiV2.Middlewares;

namespace SpeccourierApiV2.Extensions
{
	public static class MiddlewareExtensions
	{
		public static IApplicationBuilder UseGeneralExceptionMiddleware(this IApplicationBuilder builder)
		{
			return builder.UseMiddleware<GeneralExceptionMiddleware>();
		}

		public static IApplicationBuilder UseSerilogMiddleware(this IApplicationBuilder builder)
		{
			return builder.UseMiddleware<SerilogMiddleware>();
		}
	}
}
