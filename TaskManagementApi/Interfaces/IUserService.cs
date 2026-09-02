using TaskManagementApi.Models;

namespace TaskManagementApi.Interfaces;

public interface IUserService
{
    Task<User> CreateUser(User user);
}