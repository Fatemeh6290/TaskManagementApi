namespace TaskManagementApi.DTOs;

public class TaskResponseDto
{
    public int? TaskId { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public bool? IsCompleted { get; set; }
    public int? UserId { get; set; }
    public string? Name { get; set; }
}