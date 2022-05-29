using Lobby.Hubs;
using Lobby.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
     {
         options.RequireHttpsMetadata = false;
         options.TokenValidationParameters = new TokenValidationParameters
         {
             ValidateIssuer = true,
             ValidIssuer = AuthOptions.ISSUER,
             ValidateAudience = true,
             ValidAudience = AuthOptions.AUDIENCE,
             ValidateLifetime = true,
             IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
             ValidateIssuerSigningKey = true,
         };
         options.Events = new JwtBearerEvents
         {
             OnMessageReceived = context =>
             {
                 var accessToken = context.Request.Query["access_token"];

                            // если запрос направлен хабу
                            var path = context.HttpContext.Request.Path;
                 if (!string.IsNullOrEmpty(accessToken) &&
                     (path.StartsWithSegments("/chat")))
                 {
                                // получаем токен из строки запроса
                                context.Token = accessToken;
                 }
                 return Task.CompletedTask;
             }
         };
     });

builder.Services.AddSignalR();
builder.Services.AddCors();

var app = builder.Build();

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseCors(builder =>
{
    builder.WithOrigins("https://localhost:7038")
    .AllowAnyHeader()
    .WithMethods("GET", "POST")
    .AllowCredentials();
});


app.Run();