var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSignalR();
builder.Services.AddCors();

var app = builder.Build();

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseRouting();

app.UseCors(builder =>
{
    builder.WithOrigins("https://localhost:7104")
    .AllowAnyHeader()
    .WithMethods("GET", "POST")
    .AllowCredentials();
});

/*
app.UseEndpoints(endpoints =>
{
    endpoints.MapHub<EditorHub>("/editor");
});
*/

app.Run();