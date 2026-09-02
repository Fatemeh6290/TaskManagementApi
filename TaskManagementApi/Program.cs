using Microsoft.EntityFrameworkCore;
using TaskManagementApi.Data;
using TaskManagementApi.DTOs;
using TaskManagementApi.Interfaces;
using TaskManagementApi.Models;
using TaskManagementApi.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<TaskDbContext>(option =>
    option.UseSqlite(
        builder.Configuration.GetConnectionString("TaskManagementDb"))); 

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<ITaskService, TaskService>();
builder.Services.AddScoped<IUserService, UserService>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapPost("/tasks", async (CreateTaskDto dto, ITaskService taskService, TaskDbContext context) =>
{
    var user = context.Users.FirstOrDefault(u => u.UserId == dto.UserId);
    if (user == null)
        return Results.NotFound("User not found");
    
    var task = new TaskItem
    {
        Title = dto.Title,
        Description = dto.Description,
        IsCompleted = dto.IsCompleted,
        UserId = dto.UserId
    };
    await taskService.CreateTask(task);
    
    return Results.Ok();
});

app.MapPost("/users", async (CreateUserDto dto, IUserService userService) =>
{
    var user = new User
    {
        Name = dto.Name
    }; 
    await userService.CreateUser(user);
    
    return Results.Ok(user);
});

app.MapPut("/tasks/{id}", async (int id, CreateTaskDto dto, ITaskService taskService) =>
{
    var task = new TaskItem
    {
        Title = dto.Title,
        Description = dto.Description,
        IsCompleted = dto.IsCompleted,
        UserId = dto.UserId
    };
    
    var updateTask = await taskService.UpdateTask(id, task);
    
    if (updateTask is null)
        return Results.NotFound("Task not found");
    
    var result = new TaskResponseDto
    {
        TaskId = updateTask.TaskId,
        Title = updateTask.Title,
        Description = updateTask.Description,
        IsCompleted = updateTask.IsCompleted,
        UserId = updateTask.UserId,
        Name = updateTask.User.Name
    };
    
    return Results.Ok(result);
});

app.MapGet("/tasks", async (bool? isCompleted, string? title, string? sortBy, bool? descending, ITaskService taskService, int page = 1, int pageSize = 5) =>
{
    if (page < 1 || page > 5)
        return Results.BadRequest("Page must be between 1 and 5");
    
    var tasks = await taskService.GetAllTasks(isCompleted, title, sortBy, descending, page, pageSize);
    
    return Results.Ok(tasks);
});

app.MapDelete("/tasks/{id}", async (int id, ITaskService taskService) =>
{
    bool deleted = await taskService.DeleteTask(id);
    if (deleted)
        return Results.NoContent();

    return Results.NotFound();
});

app.MapGet("/tasks/{id}", async (int id, ITaskService taskService) =>
{
    var findTask = await taskService.GetTaskById(id);
    
    if(findTask is null)
        return Results.NotFound("Not Found");
    
    var result = new TaskResponseDto
    {
        TaskId = findTask?.TaskId,
        Title = findTask?.Title,
        Description = findTask?.Description,
        IsCompleted = findTask?.IsCompleted,
        UserId = findTask?.UserId,
        Name = findTask?.User.Name
    };
    
    return Results.Ok(result);
});

app.Run();
