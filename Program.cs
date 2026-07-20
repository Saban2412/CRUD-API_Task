var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => new
{
    name = "Task API", version = "1.0", endpoints = new[] {"/tasks"}
});

app.MapGet("/health", ()=> new
{
    status = "ok"
});

app.Run();
