using Microsoft.EntityFrameworkCore;
using TaskManagementApi.Data;
using TaskManagementApi.DTOs;
using TaskManagementApi.Interfaces;
using TaskManagementApi.Models;

namespace TaskManagementApi.Services;

public class TaskService : ITaskService
{
    private readonly TaskDbContext _context;

    public TaskService (TaskDbContext context)
    {
        _context = context;
    }
    public async Task<PagedResultDto> GetAllTasks(bool? isCompleted, string? title, string? sortBy, bool? descending, int page, int pageSize)
    {
        var query = _context.Tasks.AsNoTracking().AsQueryable();
        
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
                        query = query.OrderByDescending(t => t.TaskId);
                    else
                        query = query.OrderBy(t => t.TaskId);
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
        
        var totalCount = await query.CountAsync();
        var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);
        var tasks = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

        return new PagedResultDto{
            Item = tasks,
            Page = page,
            PageSize = pageSize,
            TotalCount = totalCount,
            TotalPages = totalPages
        };
    }

    public async Task<TaskItem?> GetTaskById(int id)
    {
        return await _context.Tasks.AsNoTracking().Include(u => u.User).FirstOrDefaultAsync(t => t.TaskId == id);
    }

    public async Task CreateTask(TaskItem task)
    { 
        await _context.Tasks.AddAsync(task);
        await _context.SaveChangesAsync();
    }

    public async Task<TaskItem?> UpdateTask(int id, TaskItem task)
    {
        var taskToUpdate = await _context.Tasks.FirstOrDefaultAsync(t => t.TaskId == id);
        
        if (taskToUpdate is null)
            return null;
        
        taskToUpdate.Title = task.Title;
        taskToUpdate.Description = task.Description;
        taskToUpdate.IsCompleted = task.IsCompleted;
        taskToUpdate.UserId = task.UserId;
        
        await _context.SaveChangesAsync();
        
        return await _context.Tasks.Include(u => u.User).FirstOrDefaultAsync(t => t.TaskId == id);
    }

    public async Task<bool> DeleteTask(int id)
    {
        var taskToDelete = await _context.Tasks.FirstOrDefaultAsync(t => t.TaskId == id);

        if (taskToDelete is null)
            return false;

        _context.Tasks.Remove(taskToDelete);
        await _context.SaveChangesAsync();
            
        return true;
    }
}