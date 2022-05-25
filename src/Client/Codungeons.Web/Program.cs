var builder = WebApplication.CreateBuilder();

builder.Services.AddRazorPages();
builder.Services.AddMvc(options => options.OutputFormatters.Add(new HtmlOutputFormatter()));

var app = builder.Build();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Map("/", async (context) => context.Response.Redirect("index.html"));

app.Run();