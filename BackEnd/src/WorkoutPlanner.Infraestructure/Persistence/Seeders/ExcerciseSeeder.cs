using WorkoutPlanner.Domain;
using WorkoutPlanner.Infraestructure.Persistence;
using WorkoutPlanner.Domain;

namespace WorkoutPlanner.Infrastructure.Persistence.Seeders;

public static class ExerciseSeeder
{
    public static async Task SeedAsync(AppDbContext context)
    {
        if (context.Excercises.Any())
            return;

        var exercises = new List<Excercise>
        {
            new() { Id = Guid.NewGuid(), Name = "Back Squat" },
            new() { Id = Guid.NewGuid(), Name = "Front Squat" },
            new() { Id = Guid.NewGuid(), Name = "Box Squat" },
            new() { Id = Guid.NewGuid(), Name = "Bulgarian Split Squat" },
            new() { Id = Guid.NewGuid(), Name = "Walking Lunges" },
            new() { Id = Guid.NewGuid(), Name = "Step Ups" },
            new() { Id = Guid.NewGuid(), Name = "Trap Bar Deadlift" },
            new() { Id = Guid.NewGuid(), Name = "Romanian Deadlift" },
            new() { Id = Guid.NewGuid(), Name = "Conventional Deadlift" },
            new() { Id = Guid.NewGuid(), Name = "Single Leg Romanian Deadlift" },

            new() { Id = Guid.NewGuid(), Name = "Bench Press" },
            new() { Id = Guid.NewGuid(), Name = "Incline Bench Press" },
            new() { Id = Guid.NewGuid(), Name = "Dumbbell Bench Press" },
            new() { Id = Guid.NewGuid(), Name = "Push Press" },
            new() { Id = Guid.NewGuid(), Name = "Strict Overhead Press" },
            new() { Id = Guid.NewGuid(), Name = "Landmine Press" },
            new() { Id = Guid.NewGuid(), Name = "Dips" },
            new() { Id = Guid.NewGuid(), Name = "Push Ups" },

            new() { Id = Guid.NewGuid(), Name = "Pull Ups" },
            new() { Id = Guid.NewGuid(), Name = "Chin Ups" },
            new() { Id = Guid.NewGuid(), Name = "Barbell Row" },
            new() { Id = Guid.NewGuid(), Name = "Dumbbell Row" },
            new() { Id = Guid.NewGuid(), Name = "Seated Cable Row" },
            new() { Id = Guid.NewGuid(), Name = "Lat Pulldown" },
            new() { Id = Guid.NewGuid(), Name = "Face Pull" },

            new() { Id = Guid.NewGuid(), Name = "Power Clean" },
            new() { Id = Guid.NewGuid(), Name = "Hang Clean" },
            new() { Id = Guid.NewGuid(), Name = "Clean Pull" },
            new() { Id = Guid.NewGuid(), Name = "Snatch Pull" },
            new() { Id = Guid.NewGuid(), Name = "High Pull" },
            new() { Id = Guid.NewGuid(), Name = "Jump Shrug" },
            new() { Id = Guid.NewGuid(), Name = "Medicine Ball Slam" },
            new() { Id = Guid.NewGuid(), Name = "Medicine Ball Chest Pass" },
            new() { Id = Guid.NewGuid(), Name = "Rotational Med Ball Throw" },
            new() { Id = Guid.NewGuid(), Name = "Box Jump" },

            new() { Id = Guid.NewGuid(), Name = "Hip Thrust" },
            new() { Id = Guid.NewGuid(), Name = "Glute Bridge" },
            new() { Id = Guid.NewGuid(), Name = "Nordic Hamstring Curl" },
            new() { Id = Guid.NewGuid(), Name = "Hamstring Curl" },
            new() { Id = Guid.NewGuid(), Name = "Calf Raise" },
            new() { Id = Guid.NewGuid(), Name = "Copenhagen Plank" },

            new() { Id = Guid.NewGuid(), Name = "Plank" },
            new() { Id = Guid.NewGuid(), Name = "Side Plank" },
            new() { Id = Guid.NewGuid(), Name = "Dead Bug" },
            new() { Id = Guid.NewGuid(), Name = "Pallof Press" },
            new() { Id = Guid.NewGuid(), Name = "Ab Wheel Rollout" },

            new() { Id = Guid.NewGuid(), Name = "Farmer Carry" },
            new() { Id = Guid.NewGuid(), Name = "Suitcase Carry" },
            new() { Id = Guid.NewGuid(), Name = "Sled Push" },
            new() { Id = Guid.NewGuid(), Name = "Sled Pull" }
        };

        await context.Excercises.AddRangeAsync(exercises);
        await context.SaveChangesAsync();
    }
}