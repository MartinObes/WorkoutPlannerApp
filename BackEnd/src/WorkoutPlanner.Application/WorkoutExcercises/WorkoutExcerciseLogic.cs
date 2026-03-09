using WorkoutPlanner.Domain;

namespace WorkoutPlanner.Application.WorkoutExcercises;

public class WorkoutExcerciseLogic: IWorkoutExcerciseLogic
{
 public WorkoutExcercise createWorkoutExcercise(string name, Guid workoutId, Guid excerciseId, int reps, int sets,
     Enums.LoadType loadType, int? weight = null, int? percentage = null)
     
    public WorkoutExcercise updateWorkoutExcercise(WorkoutExcercise workoutExcercise, string name, Guid workoutId,
        Guid excerciseId, int reps, int sets, Enums.LoadType loadType, int? weight = null, int? percentage = null)
    {
        throw new NotImplementedException();
    }

    public void deleteWorkoutExcercise(WorkoutExcercise workoutExcercise)
    {
        throw new NotImplementedException();
    }
}