using TaskManagementApi.Models;

namespace TaskManagementApi.DTOs;

public class PagedResultDto
{
    public List<TaskItem> Item { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalCount { get; set; }
    public int TotalPages { get; set; }
}