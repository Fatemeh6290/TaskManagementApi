using TaskManagementApi.DTOs;
using TaskManagementApi.Models;

namespace TaskManagementApi.Interfaces;

public interface ITaskService
{
    PagedResultDto GetAllTasks(bool? isCompleted, string? title, string? sortBy, bool? descending, int page, int pageSize);
    TaskItem? GetTaskById(int id);
    void CreateTask(TaskItem task);
    TaskItem? UpdateTask(int id, TaskItem task);
    bool DeleteTask(int id);
}