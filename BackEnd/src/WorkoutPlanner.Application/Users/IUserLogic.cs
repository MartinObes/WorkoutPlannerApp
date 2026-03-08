using WorkoutPlanner.Domain;

namespace WorkoutPlanner.Application.Users;

public interface IUserLogic
{
    public User CreateUser(string password, string name, string surname, string email, Enums.UserRole role);
    public User UpdateUser(User user, string? password, string? name, string? surname, string? email, Enums.UserRole? role);
    public void DeleteUser(User user);
}