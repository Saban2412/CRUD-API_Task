var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

var tasks = new List<TaskItem>
{
    new TaskItem { Id = 1, Title = "Learn ASP.NET Core", Done = false },
    new TaskItem { Id = 2, Title = "Build a Minimal API", Done = false },
    new TaskItem { Id = 3, Title = "Push project to GitHub", Done = false }
};

//Fist Endpoint
app.MapGet("/", () => new
{
    name = "Task API", version = "1.0", endpoints = new[] {"/tasks"}
});

//Health endpoint
app.MapGet("/health", ()=> new
{
    status = "ok"
});

//GetAll()
app.MapGet("/tasks",()=>tasks);

//GetById()
app.MapGet("/tasks/{id}",(int id) =>
{
    var task = tasks.FirstOrDefault(t=>t.Id==id);

    if(task is null)
    {
        return Results.NotFound(new
        {
            error = $"Task {id} not found"
        });
    }
    return Results.Ok(task);
});

//CreateNew()
app.MapPost("/tasks",(CreateTaskRequest request) =>
{
    if (string.IsNullOrWhiteSpace(request.Title))
    {
        return Results.BadRequest(new
        {
            error = "Title is required"
        });
    } 

    var newtask = new TaskItem
    {
      Id = tasks.Max(t=>t.Id)+1,
      Title = request.Title,
      Done = false  
    };
    tasks.Add(newtask);

    return Results.Created($"/tasks/{newtask.Id}",newtask);
});

app.Run();

class TaskItem
{
    public int Id{get;set;}
    public string Title{get;set;} = string.Empty;
    public bool Done{get;set;}
}
class CreateTaskRequest
{
    public string? Title{get;set;}
}