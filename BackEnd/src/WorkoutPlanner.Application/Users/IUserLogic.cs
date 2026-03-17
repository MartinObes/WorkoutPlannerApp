using WorkoutPlanner.Domain;

namespace WorkoutPlanner.Application.Users;

public interface IUserLogic
{
    public Task<User> CreateUser(string password, string name, string surname, string email, Enums.UserRole role);
    public Task<User?> LoginUser(string email, string password);
    public Task<User> UpdateUser(string username, string? password, string? name, string? surname, string? email, Enums.UserRole? role);
    public Task DeleteUser(string username);
    public Task<bool> UserExists(string name);
    public Task<User> GetUserByName(string name);
    public Task<IList<User>> GetAllUsers();
}