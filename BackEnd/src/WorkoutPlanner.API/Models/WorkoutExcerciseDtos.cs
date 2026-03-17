using WorkoutPlanner.Domain;

namespace WorkoutPlanner.API.Models;

public class UpdateWorkoutExcerciseRequestDto
{
    public Guid WorkoutExcerciseId { get; set; }
    public string Name { get; set; } = string.Empty;
    public Guid WorkoutId { get; set; }
    public Guid ExcerciseId { get; set; }
    public int Reps { get; set; }
    public int Sets { get; set; }
    public Enums.LoadType LoadType { get; set; }
    public int? Weight { get; set; }
    public int? Percentage { get; set; }
}

public class WorkoutExcerciseResponseDto
{
    public Guid Id { get; set; }
    public Guid WorkoutId { get; set; }
    public Guid ExcerciseId { get; set; }
    public string ExcerciseName { get; set; } = string.Empty;
    public int Reps { get; set; }
    public int Sets { get; set; }
    public Enums.LoadType LoadType { get; set; }
    public int? Weight { get; set; }
    public int? Percentage { get; set; }

    public WorkoutExcerciseResponseDto()
    {
    }

    public WorkoutExcerciseResponseDto(WorkoutExcercise workoutExcercise)
    {
        Id = workoutExcercise.Id;
        WorkoutId = workoutExcercise.WorkoutId;
        ExcerciseId = workoutExcercise.ExcerciseId;
        ExcerciseName = workoutExcercise.Excercise?.Name ?? string.Empty;
        Reps = workoutExcercise.Reps;
        Sets = workoutExcercise.Sets;
        LoadType = workoutExcercise.LoadType;
        Weight = workoutExcercise.Weight;
        Percentage = workoutExcercise.Percentage;
    }
}

public class WorkoutExcercisesResponseDto
{
    public IList<WorkoutExcerciseResponseDto> WorkoutExcercises { get; set; } = new List<WorkoutExcerciseResponseDto>();

    public WorkoutExcercisesResponseDto()
    {
    }

    public WorkoutExcercisesResponseDto(IList<WorkoutExcercise> workoutExcercises)
    {
        WorkoutExcercises = workoutExcercises.Select(workoutExcercise => new WorkoutExcerciseResponseDto(workoutExcercise)).ToList();
    }
}
