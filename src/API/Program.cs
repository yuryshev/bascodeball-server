using API.Extensions;
using App.Metrics.Health.Builder;
using App.Metrics.Health.Checks.Sql;
using Common.ConvertingModel;
using Npgsql;


var builder = WebApplication.CreateBuilder(args);

var pgConnectionString = builder.Configuration.GetConnectionString("PostgresConnection");
var serviceCollectionExtensions = new ServiceCollectionExtensions(builder.Services);
serviceCollectionExtensions.AddPgDbContext(pgConnectionString);
serviceCollectionExtensions.AddApiServices(builder.Configuration);

builder.Services.AddControllers();

var healthRoot = new HealthBuilder()
    .OutputHealth.Using<HealthOutputJsonFormatter>()
    .HealthChecks.RegisterFromAssembly(builder.Services)
    .HealthChecks.AddSqlCachedCheck(
        "Pg Database",
        () => new NpgsqlConnection(pgConnectionString),
        timeout: TimeSpan.FromSeconds(2),
        cacheDuration: TimeSpan.FromSeconds(30))
    .Build();

builder.Services.AddHealth(healthRoot);

var app = builder.Build();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseHealthAllEndpoints();

app.Run();