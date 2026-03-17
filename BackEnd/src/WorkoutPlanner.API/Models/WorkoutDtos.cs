using WorkoutPlanner.Domain;
using WorkoutPlanner.Domain.AuxiliaryDomainClasses;

namespace WorkoutPlanner.API.Models;

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
