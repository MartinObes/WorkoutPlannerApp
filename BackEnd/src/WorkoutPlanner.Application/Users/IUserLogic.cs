using WorkoutPlanner.Domain;

namespace WorkoutPlanner.Application.Users;

public interface IUserLogic
{
    public Task<User> CreateUser(string password, string name, string surname, string email, Enums.UserRole role);
    public User UpdateUser(User user, string? password, string? name, string? surname, string? email, Enums.UserRole? role);
    public void DeleteUser(User user);
    public Task<bool> UserExists(string name);
     public Task<User> GetUserByName(string name);
     public Task<IList<User>> GetAllUsers();
}