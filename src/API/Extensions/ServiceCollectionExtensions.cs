﻿using API.Data;
using Microsoft.EntityFrameworkCore;

namespace API.Extensions;
public class ServiceCollectionExtensions
{
    private IServiceCollection _collection { get; set; }

    public ServiceCollectionExtensions(IServiceCollection collection)
    {
       _collection = collection;
    }

    
    public void AddApiServices(IConfiguration configuration)
    {
        
    }

    public void AddPgDbContext(string connectionString)
    {
        if (string.IsNullOrEmpty(connectionString))
        {
            throw new ArgumentException("Connection string is null or empty.", nameof(connectionString));
        }

        _collection.AddDbContextPool<PgDbContext>(builder => builder.UseNpgsql(
            connectionString, npSqlBuilder =>
            {
                npSqlBuilder.SetPostgresVersion(new Version(13, 0));
                npSqlBuilder.EnableRetryOnFailure();
            }));
    }
}
