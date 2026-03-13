using WorkoutPlanner.Domain;

namespace WorkoutPlanner.Application.WorkoutExcercises;

public interface IWorkoutExcerciseLogic
{
    public Task<WorkoutExcercise> createWorkoutExcercise( Guid workoutId, Guid excerciseId, int reps, int sets, Enums.LoadType loadType, int? weight = null, int? percentage = null);
     public WorkoutExcercise updateWorkoutExcercise(WorkoutExcercise workoutExcercise, string name, Guid workoutId, Guid excerciseId, int reps, int sets, Enums.LoadType loadType, int? weight = null, int? percentage = null);
     public void deleteWorkoutExcercise( WorkoutExcercise workoutExcercise);
     public Task<WorkoutExcercise> getWorkoutExcerciseById(Guid id);
     public Task<IList<WorkoutExcercise>> getAllWorkoutExcercises();
}