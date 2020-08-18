using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication10.Infrastructure.StartUpExtentions
{
	public static class StartupServices
	{
		public static IServiceCollection AddCustomHealthChecks(this IServiceCollection services)
		{
			var hcBuilder = services.AddHealthChecks();

			hcBuilder.AddCheck("self", () => HealthCheckResult.Healthy());

			// Add Mongo BD check

			//hcBuilder
			//	.add(
			//		configuration["ConnectionString"],
			//		name: "dB-check",
			//		tags: new string[] { "orderingdb" });


			return services;
		}

		public static IServiceCollection AddCustomSwagger(this IServiceCollection services)
		{
			services.AddSwaggerGen(options =>
			{
				options.SwaggerDoc("v1", new OpenApiInfo
				{
					Title = "Service1",
					Version = "v1",
					Description = "The Service1 HTTP API"
				});
			});

			return services;
		}

		public static IServiceCollection AddCustomMvc(this IServiceCollection services)
		{
			// Add framework services.
			services.AddControllers();
			return services;
		}
	}
}
