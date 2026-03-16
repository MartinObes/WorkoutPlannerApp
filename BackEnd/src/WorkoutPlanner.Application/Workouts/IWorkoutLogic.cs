using WorkoutPlanner.Domain;
using WorkoutPlanner.Domain.AuxiliaryDomainClasses;

namespace WorkoutPlanner.Application.Workouts;

public interface IWorkoutLogic
{
    public Task<Workout> CreateWorkout(string name, Guid? coachId,IList<CreateWorkoutExcerciseArgs> workoutExcerciseArgsList);
    public Task<Workout> UpdateWorkout(Guid workoutid, string name, Guid? coachId);
    public Task  DeleteWorkout(string name);
    public Task<bool> WorkoutExists(string name);
    public Task<Workout> GetWorkoutByName(string name);
    public Task<IList<Workout>> GetAllWorkouts();
}