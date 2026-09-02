using System.ComponentModel.DataAnnotations;

namespace TaskManagementApi.Models;

public class TaskItem()
{
    [Key]
    public int TaskId { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public bool IsCompleted { get; set; }
    public int UserId { get; set; }
    public User User { get; set; }
}
