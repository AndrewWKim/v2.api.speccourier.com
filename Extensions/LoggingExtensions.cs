using Serilog;
using Serilog.Events;

namespace SpeccourierApiV2.Extensions
{
	public class LoggingExtensions
	{
		public static void ConfigureLogging()
		{
			const LogEventLevel minimumLogLevel = LogEventLevel.Information;

			Log.Logger = new LoggerConfiguration()
				.MinimumLevel.Is(minimumLogLevel)
				.MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
				.MinimumLevel.Override("System", LogEventLevel.Warning)
				.MinimumLevel.Override("Microsoft.AspNetCore.Authentication", LogEventLevel.Information)
				.MinimumLevel.Override("Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerHandler", LogEventLevel.Warning)
				.MinimumLevel.Override("IdentityServer4.AccessTokenValidation", LogEventLevel.Warning)
				.Enrich.FromLogContext()
				.Enrich.WithThreadId()
				.Enrich.WithMachineName()
				.WriteTo.Async(a =>
				{
					a.RollingFile(
						"Logs\\orprep-{Date}.txt",
						minimumLogLevel,
						"[{Timestamp:HH:mm:ss.fff} {Level:u3}] {Message:lj} {NewLine}{Properties:j} {NewLine}{Exception}");
				})
				.CreateLogger();
		}
	}
}
