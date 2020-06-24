using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SpeccourierApiV2.Core.Configurations;
using Newtonsoft.Json;
using AutoMapper;
using SpeccourierApiV2.Core.Services;
using SpeccourierApiV2.Extensions;
using SpeccourierApiV2.Services;
using SpeccourierApiV2.Data.Context;

namespace SpeccourierApiV2
{
	public class Startup
	{
		public Startup(IWebHostEnvironment env)
		{
			var builder = new ConfigurationBuilder()
			   .SetBasePath(env.ContentRootPath)
			   .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
			   .AddEnvironmentVariables();

			Configuration = builder.Build();
			ProjectConfiguration = GetSpecCourierConfig();
		}

		public IConfiguration Configuration { get; }

		public SpecCourierConfig ProjectConfiguration { get; set; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			var config = ProjectConfiguration;
			services.AddSingleton(typeof(SpecCourierConfig), config);
			services.AddControllers().AddNewtonsoftJson(x => x.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);
			
			services.AddDbContext<SpecCourierContext>();
			services.AddScoped<ISpecCourierContext>(provider => provider.GetService<SpecCourierContext>());

			services.AddAutoMapper(typeof(Startup));
			services.AddAuthorization();

			services.AddCors(o => o.AddPolicy("AllowAnyOrigin", builder =>
			{
				builder.AllowAnyMethod()
					.AllowAnyHeader()
					.AllowAnyOrigin();
			}));

			services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
				{
					Title = "SpecCourier Swagger",
					Description = "SpecCourier API V2"
				});
			});

			RegisterServices(services);
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseRouting();

			app.UseAuthorization();

			app.UseGeneralExceptionMiddleware();
			app.UseSerilogMiddleware();

			app.UseSwagger();
			app.UseSwaggerUI(c =>
			{
				c.SwaggerEndpoint("/swagger/v1/swagger.json", "Swagger Sample");
			});

			app.UseCors("AllowAnyOrigin");

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});
		}

		private void RegisterServices(IServiceCollection services)
		{
			services.AddScoped<IPackageService, PackageService>();
		}

		private SpecCourierConfig GetSpecCourierConfig()
		{
			var config = new SpecCourierConfig();
			Configuration.GetSection("SpecCourier").Bind(config);

			return config;
		}
	}
}
