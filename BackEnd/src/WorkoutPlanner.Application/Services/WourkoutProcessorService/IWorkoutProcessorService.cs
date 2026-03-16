using WorkoutPlanner.Domain.AuxiliaryDomainClasses;

namespace WorkoutPlanner.Application.Services.WourkoutProcessorService;

public interface IWorkoutProcessorService
{
    Task<List<ProcessedExercise>> ProcessWorkout(string workoutName, string username);
}