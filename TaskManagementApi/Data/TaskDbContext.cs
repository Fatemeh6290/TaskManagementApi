using Microsoft.EntityFrameworkCore;
using TaskManagementApi.Models;

namespace TaskManagementApi.Data;

public class TaskDbContext : DbContext
{
    public TaskDbContext (DbContextOptions<TaskDbContext> options) : base(options)
    {
        
    }
    
    public DbSet<TaskItem> Tasks { get; set; }
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TaskItem>()
            .HasOne(u => u.User)
            .WithMany(u => u.Tasks)
            .HasForeignKey(u => u.UserId);
    }
}