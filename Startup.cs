using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Ben.Diagnostics;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using WebApplication10.Infrastructure.AutofacModules;
using Microsoft.OpenApi.Models;
using WebApplication10.Infrastructure.StartUpExtentions;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

namespace WebApplication10
{
	public class Startup
	{
		public Startup(IConfiguration configuration, IWebHostEnvironment env)
		{
			Configuration = configuration;
			HostingEnvironment = env;
		}

		public IConfiguration Configuration { get; }
		public IWebHostEnvironment HostingEnvironment { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddGrpc(options =>
					{
						options.EnableDetailedErrors = HostingEnvironment.IsDevelopment();
					})
					.Services
					.AddCustomMvc()
					.AddCustomHealthChecks()
					.AddCustomSwagger();

			services.AddSwaggerGen();
		}

		public void ConfigureContainer(ContainerBuilder builder)
		{
			//configure autofac

			builder.RegisterModule(new MediatorModule());
			builder.RegisterModule(new ApplicationModule());
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			// Add blocking detector at start of Configure
			app.UseBlockingDetection();

			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseHttpsRedirection();

			app.UseSwagger();

			// Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
			// specifying the Swagger JSON endpoint.
			app.UseSwaggerUI(c =>
			{
				c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
			});

			app.UseRouting();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
				app.UseHealthChecks("/self", new HealthCheckOptions
				{
					Predicate = r => r.Name.Contains("self")
				});
			});
		}
	}
}
