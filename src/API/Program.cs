using API.Extensions;
using API.Models.AuthModels;
using App.Metrics.Health.Builder;
using App.Metrics.Health.Checks.Sql;
using Common.ConvertingModel;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
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

builder.Services.AddAuthorization();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            // указывает, будет ли валидироваться издатель при валидации токена
            ValidateIssuer = true,
            // строка, представляющая издателя
            ValidIssuer = AuthOptions.ISSUER,
            // будет ли валидироваться потребитель токена
            ValidateAudience = true,
            // установка потребителя токена
            ValidAudience = AuthOptions.AUDIENCE,
            // будет ли валидироваться время существования
            ValidateLifetime = true,
            // установка ключа безопасности
            IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
            // валидация ключа безопасности
            ValidateIssuerSigningKey = true,
        };
    });

var app = builder.Build();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseHealthAllEndpoints();

app.Run();