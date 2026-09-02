using System.ComponentModel.DataAnnotations;

namespace TaskManagementApi.Models;

public class User
{
    [Key]
    public int UserId { get; set; }
    public string Name { get; set; }
    public List<TaskItem> Tasks { get; set; } = new();
}