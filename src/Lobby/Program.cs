using Lobby.Hubs;
using Lobby.Services;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddSignalR();
builder.Services.AddCors();
builder.Services.AddSingleton<GroupService>();
builder.Services.AddSingleton<CodeTaskService>();

var app = builder.Build();

app.UseHttpsRedirection();

app.UseRouting();

app.UseCors(builder =>
{
    builder.WithOrigins("http://localhost:3000")
    .AllowAnyHeader()
    .AllowAnyMethod()
    .AllowCredentials();
});

app.UseEndpoints(endpoints =>
{
    endpoints.MapHub<LobbyHub>("/lobby");
});


app.Run();