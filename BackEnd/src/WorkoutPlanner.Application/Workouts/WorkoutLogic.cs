using WorkoutPlanner.Domain;

namespace WorkoutPlanner.Application.Workouts;

public class WorkoutLogic : IWorkoutLogic
{
    public Workout createWorkout(string name, Guid? coachId)
    {
        if(string.IsNullOrEmpty(name))
            throw new ArgumentException("Name cannot be empty");
        
        return new Workout
        {
            Name = name,
            CoachId = coachId
        };
    }
    
    public Workout updateWorkout(Workout workout, string name)
    {
        if(string.IsNullOrEmpty(name))
            throw new ArgumentException("Name cannot be empty");
        
        workout.Name = name;
        return workout;
    }
}