using static WorkoutPlanner.Domain.Enums;

namespace WorkoutPlanner.Domain;

public class User
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public UserRole Role { get; set; }
    
    public static void ValidateRole(UserRole role)
    {
        if (!Enum.IsDefined(typeof(UserRole), role))
        {
            throw new ArgumentException("Invalid user role.");
        }
    }
}
