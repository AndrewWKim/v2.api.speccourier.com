using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace SpeccourierApiV2.Extensions
{
	public static class HttpContextExtensions
	{
		public static async Task<string> GetRequestBodyString(this HttpContext httpContext)
		{
			var body = string.Empty;
			if (httpContext.Request.ContentLength == null
			    || !(httpContext.Request.ContentLength > 0))
			{
				return body;
			}

			httpContext.Request.EnableBuffering();
			using (var reader = new StreamReader(httpContext.Request.Body, Encoding.UTF8, true, 1024, true))
			{
				body = await reader.ReadToEndAsync();
			}

			httpContext.Request.Body.Position = 0;
			return body;
		}
	}
}
