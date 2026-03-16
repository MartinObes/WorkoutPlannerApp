namespace WorkoutPlanner.Application.Services.WourkoutProcessorService;

public interface IWorkoutProcessorService
{
    void ProcessWorkout(string workoutName, string username);
}