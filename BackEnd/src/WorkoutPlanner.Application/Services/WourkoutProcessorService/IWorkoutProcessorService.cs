using WorkoutPlanner.Domain.AuxiliaryDomainClasses;

namespace WorkoutPlanner.Application.Services.WourkoutProcessorService;

public interface IWorkoutProcessorService
{
    Task<IList<ProcessedExercise>> ProcessWorkout(string workoutName, string username);
}