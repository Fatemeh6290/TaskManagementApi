using TaskManagementApi.Data;
using TaskManagementApi.Interfaces;
using TaskManagementApi.Models;

namespace TaskManagementApi.Services;

public class UserService : IUserService
{
    private readonly TaskDbContext _context;

    public UserService(TaskDbContext context)
    {
        _context = context;
    }
    public async Task<User> CreateUser(User user)
    {
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
        
        return user;
    }
}