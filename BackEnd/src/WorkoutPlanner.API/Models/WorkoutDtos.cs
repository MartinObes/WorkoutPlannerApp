using WorkoutPlanner.Domain;
using WorkoutPlanner.Domain.AuxiliaryDomainClasses;

namespace WorkoutPlanner.API.Models;

public class CreateWorkoutExerciseArgRequestDto
{
    public Guid ExcerciseId { get; set; }
    public int Sets { get; set; }
    public int Reps { get; set; }
    public Enums.LoadType LoadType { get; set; }
    public int? Weight { get; set; }
    public int? Percentage { get; set; }
}

public class CreateWorkoutRequestDto
{
    public string Name { get; set; } = string.Empty;
    public Guid? CoachId { get; set; }
    public IList<CreateWorkoutExcerciseArgs> WorkoutExcercises { get; set; } = new List<CreateWorkoutExcerciseArgs>();
}

public class UpdateWorkoutRequestDto
{
    public Guid WorkoutId { get; set; }
    public string Name { get; set; } = string.Empty;
    public Guid? CoachId { get; set; }
}

public class DeleteWorkoutRequestDto
{
    public string Name { get; set; } = string.Empty;
}

public class WorkoutExistsRequestDto
{
    public string Name { get; set; } = string.Empty;
}

public class GetWorkoutByNameRequestDto
{
    public string Name { get; set; } = string.Empty;
}

public class WorkoutResponseDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public Guid? CoachId { get; set; }
    public IList<WorkoutExcerciseResponseDto> WorkoutExcercises { get; set; } = new List<WorkoutExcerciseResponseDto>();

    public WorkoutResponseDto()
    {
    }

    public WorkoutResponseDto(Workout workout)
    {
        Id = workout.Id;
        Name = workout.Name;
        CoachId = workout.CoachId;
        WorkoutExcercises = workout.WorkoutExcercises.Select(workoutExcercise => new WorkoutExcerciseResponseDto(workoutExcercise)).ToList();
    }
}

public class WorkoutsResponseDto
{
    public IList<WorkoutResponseDto> Workouts { get; set; } = new List<WorkoutResponseDto>();

    public WorkoutsResponseDto()
    {
    }

    public WorkoutsResponseDto(IList<Workout> workouts)
    {
        Workouts = workouts.Select(workout => new WorkoutResponseDto(workout)).ToList();
    }
}
