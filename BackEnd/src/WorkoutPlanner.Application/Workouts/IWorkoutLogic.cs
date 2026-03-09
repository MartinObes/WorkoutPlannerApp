using WorkoutPlanner.Domain;

namespace WorkoutPlanner.Application.Workouts;

public interface IWorkoutLogic
{
    public Workout createWorkout(string name, Guid? coachId);
    public Workout updateWorkout(Workout workout, string name);
}