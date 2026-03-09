
using WorkoutPlanner.Domain;
using WorkoutPlanner.Infraestructure.Persistence;

namespace WorkoutPlanner.Infrastructure.Persistence.Seeders;

public static class UserSeeder
{
    public static async Task SeedAsync(AppDbContext context)
    {
        if (!context.Users.Any())
        {
            var coach = new User
            {
                Id = Guid.NewGuid(),
                Name = "Demo Coach",
                Email = "coach@workoutplanner.com",
                PasswordHash = "Coach123!",
                Role = Enums.UserRole.Trainer
            };

            var player = new User
            {
                Id = Guid.NewGuid(),
                Name = "Demo Player",
                Email = "player@workoutplanner.com",
                PasswordHash ="Player123!",
                Role = Enums.UserRole.Player
            };

            await context.Users.AddRangeAsync(coach, player);
            await context.SaveChangesAsync();
        }
    }
}