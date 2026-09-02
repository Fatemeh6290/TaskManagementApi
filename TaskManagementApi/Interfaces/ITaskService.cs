using TaskManagementApi.DTOs;
using TaskManagementApi.Models;

namespace TaskManagementApi.Interfaces;

public interface ITaskService
{
    Task<PagedResultDto> GetAllTasks(bool? isCompleted, string? title, string? sortBy, bool? descending, int page, int pageSize);
    Task<TaskItem?> GetTaskById(int id);
    Task CreateTask(TaskItem task);
    Task<TaskItem?> UpdateTask(int id, TaskItem task);
    Task<bool> DeleteTask(int id);
}