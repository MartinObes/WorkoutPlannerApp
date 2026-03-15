using WorkoutPlanner.Domain;
using WorkoutPlanner.Domain.AuxiliaryDomainClasses;

namespace WorkoutPlanner.Application.Workouts;

public interface IWorkoutLogic
{
    public Task<Workout> CreateWorkout(string name, Guid? coachId,IList<CreateWorkoutExcerciseArgs> workoutExcerciseArgsList);
    public Workout UpdateWorkout(Workout workout, string name, Guid? coachId);
    public void DeleteWorkout(Workout workout);
    public Task<bool> WorkoutExists(string name);
    public Task<Workout> GetWorkoutByName(string name);
    public Task<IList<Workout>> GetAllWorkouts();
}