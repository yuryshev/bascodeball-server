using System;
using System.Diagnostics.CodeAnalysis;
using Api.Data;
using App.Metrics.Health;
using App.Metrics.Health.Builder;
using App.Metrics.Health.Checks.Sql;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Npgsql;

namespace Api
{
    /// <summary>
    /// Comparator startup.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class Startup
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Startup"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        /// <summary>
        /// Gets the Configuration property.
        /// </summary>
        private IConfiguration Configuration { get; }

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services">The services.</param>
        public void ConfigureServices(IServiceCollection services)
        {
            var pgConnectionString = this.Configuration.GetConnectionString("PostgresConnection");
            services.AddPgDbContext(pgConnectionString);

            services.AddApiServices(this.Configuration);

            services.AddMvcCore()
                .SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

            var healthRoot = new HealthBuilder()
                .OutputHealth.Using<HealthOutputJsonFormatter>()
                .HealthChecks.RegisterFromAssembly(services)
                .HealthChecks.AddSqlCachedCheck(
                    "Pg Database",
                    () => new NpgsqlConnection(pgConnectionString),
                    timeout: TimeSpan.FromSeconds(2),
                    cacheDuration: TimeSpan.FromSeconds(30))
                .Build();

            services.AddHealth(healthRoot);
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app">The application.</param>
        /// <param name="env">The env.</param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedProto,
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseRouting();

            app.UseHealthAllEndpoints();

            app.UseEndpoints(builder =>
            {
                builder.MapControllers();
            });
        }
    }
}
