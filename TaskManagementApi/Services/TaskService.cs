using TaskManagementApi.DTOs;
using TaskManagementApi.Interfaces;
using TaskManagementApi.Models;

namespace TaskManagementApi.Services;

public class TaskService : ITaskService
{
    private readonly List<TaskItem> _tasks = new ();

    public TaskService ()
    {
        _tasks.Add(new TaskItem
        {
            Id = 1,
            Title = "Task 1",
            Description = "Description 1",
            IsCompleted = true
        });

        _tasks.Add(new TaskItem
        {
            Id = 2,
            Title = "Task 2",
            Description = "Description 2",
            IsCompleted = false
        });

        _tasks.Add(new TaskItem
        {
            Id = 3,
            Title = "Task 3",
            Description = "Description 3",
            IsCompleted = true
        });

        _tasks.Add(new TaskItem
        {
            Id = 4,
            Title = "Task 4",
            Description = "Description 4",
            IsCompleted = false
        });

        _tasks.Add(new TaskItem
        {
            Id = 5,
            Title = "Task 5",
            Description = "Description 5",
            IsCompleted = true
        });
    }
    public PagedResultDto GetAllTasks(bool? isCompleted, string? title, string? sortBy, bool? descending, int page, int pageSize)
    {
        var query = _tasks.AsEnumerable();
        
        if (isCompleted.HasValue)
            query = query.Where(t => t.IsCompleted == isCompleted.Value);
        
        if (!string.IsNullOrEmpty(title))
            query = query.Where(t => t.Title != null && t.Title.Contains(title));

        if (!string.IsNullOrEmpty(sortBy))
        {
            switch (sortBy)
            {   
                case "id":
                    if (descending is true)
                        query = query.OrderByDescending(t => t.Id);
                    else
                        query = query.OrderBy(t => t.Id);
                    break;
                case "title":
                    if (descending is true)
                        query = query.OrderByDescending(t => t.Title);
                    else
                        query = query.OrderBy(t => t.Title);
                    break;
                case "description":
                    if (descending is true)
                        query = query.OrderByDescending(t => t.Description);
                    else
                        query = query.OrderBy(t => t.Description);
                    break;
            }
        }
        
        var totalCount = query.Count();
        var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);
        var tasks = query.Skip((page - 1) * pageSize).Take(pageSize).ToList();

        return new PagedResultDto{
            Item = tasks,
            Page = page,
            PageSize = pageSize,
            TotalCount = totalCount,
            TotalPages = totalPages
        };
    }

    public TaskItem? GetTaskById(int id)
    {
        return _tasks.FirstOrDefault(t => t.Id == id);
    }

    public void CreateTask(TaskItem task)
    {
        _tasks.Add(task);
    }

    public TaskItem? UpdateTask(int id, TaskItem task)
    {
        var taskToUpdate = GetTaskById(id);
        if (taskToUpdate != null)
        {
            taskToUpdate.Title = task.Title;
            taskToUpdate.Description = task.Description;
            taskToUpdate.IsCompleted = task.IsCompleted;
        }
        
        return taskToUpdate;
    }

    public bool DeleteTask(int id)
    {
        var taskToDelete = GetTaskById(id);

        if (taskToDelete != null)
        {
            _tasks.Remove(taskToDelete);
            return true;
        }
        
        return false;
    }
}