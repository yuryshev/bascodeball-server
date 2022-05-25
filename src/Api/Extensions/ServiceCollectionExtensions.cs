using System;
using Api.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Contains extensions method applied to <see cref="IServiceCollection"/>.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds the comparator services.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <param name="configuration">The configuration.</param>
        /// <returns>IServiceCollection.</returns>
        public static IServiceCollection AddApiServices(this IServiceCollection services, IConfiguration configuration)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            return services;
        }

        /// <summary>
        /// Register DB context in the <see cref="IServiceCollection"/>.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <param name="connectionString">The connection string.</param>
        /// <exception cref="ArgumentNullException">services.</exception>
        /// <exception cref="ArgumentException">connectionString.</exception>
        /// <returns>Service collection.</returns>
        public static IServiceCollection AddPgDbContext(this IServiceCollection services, string connectionString)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentException("Connection string is null or empty.", nameof(connectionString));
            }

            services.AddDbContextPool<PgDbContext>(builder => builder.UseNpgsql(
                connectionString, npSqlBuilder =>
                {
                    npSqlBuilder.SetPostgresVersion(new Version(13, 0));
                    npSqlBuilder.EnableRetryOnFailure();
                }));
            return services;
        }
    }
}
