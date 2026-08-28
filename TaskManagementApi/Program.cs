using Microsoft.AspNetCore.Components.Web;
using TaskManagementApi.DTOs;
using TaskManagementApi.Interfaces;
using TaskManagementApi.Models;
using TaskManagementApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<ITaskService, TaskService>();
var app = builder.Build();

    
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapPost("/tasks", (TaskItem task, ITaskService taskService) =>
{
    taskService.CreateTask(task);
    return Results.Ok();
});

app.MapPut("/tasks/{id}", (int id,CreateTaskDto dto, ITaskService taskService) =>
{
    var taskItem = new TaskItem
        {
            Id = id, 
            Title = dto.Title, 
            Description = dto.Description, 
            IsCompleted = dto.IsCompleted
        };
    var updateTask = taskService.UpdateTask(id, taskItem);
    
    if (updateTask is not null)
        return Results.Ok(updateTask);
    
    return Results.NotFound();
});

app.MapGet("/tasks", (bool? isCompleted, string? title, string? sortBy, bool? descending, ITaskService taskService, int page = 1, int pageSize = 5) =>
{
    if (page < 1 || page > 5)
        return Results.BadRequest("Page must be between 1 and 5");
    
    var tasks = taskService.GetAllTasks(isCompleted, title, sortBy, descending, page, pageSize);
    
    return Results.Ok(tasks);
});

app.MapDelete("/tasks/{id}", (int id, ITaskService taskService) =>
{
    bool deleted = taskService.DeleteTask(id);
    if (deleted)
        return Results.NoContent();

    return Results.NotFound();
});

app.MapGet("/tasks/{id}", (int id, ITaskService taskService) =>
{
    
    var findTask = taskService.GetTaskById(id);
    if(findTask is not null)
        return Results.Ok(findTask);
    
    return Results.NotFound("Not found");

});
app.MapPost("/hello/{name}", (string name) => $"Hello {name}");

app.Run();
