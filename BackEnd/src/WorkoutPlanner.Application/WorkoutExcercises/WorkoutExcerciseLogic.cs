using WorkoutPlanner.Domain;

namespace WorkoutPlanner.Application.WorkoutExcercises;

public class WorkoutExcerciseLogic: IWorkoutExcerciseLogic
{
public WorkoutExcercise createWorkoutExcercise(string name, Guid workoutId, Guid excerciseId, int reps, int sets,
    Enums.LoadType loadType, int? weight = null, int? percentage = null)
{
    WorkoutExcercise.Validate(sets, reps, loadType, weight, percentage);

    var workoutExcercise= new WorkoutExcercise(sets, reps, loadType, weight, percentage)
    {
        WorkoutId = workoutId,
        ExcerciseId = excerciseId
    };

    return workoutExcercise;
}
     
    public WorkoutExcercise updateWorkoutExcercise(WorkoutExcercise workoutExcercise, string name, Guid workoutId,
        Guid excerciseId, int reps, int sets, Enums.LoadType loadType, int? weight = null, int? percentage = null)
    {
        if (workoutExcercise == null) throw new ArgumentException("WorkoutExcercise cannot be null.");

        WorkoutExcercise.Validate(sets, reps, loadType, weight, percentage);

        workoutExcercise.WorkoutId = workoutId;
        workoutExcercise.ExcerciseId = excerciseId;
        workoutExcercise.Reps = reps;
        workoutExcercise.Sets = sets;
        workoutExcercise.LoadType = loadType;
        workoutExcercise.Weight = weight;
        workoutExcercise.Percentage = percentage;

        return workoutExcercise;
    }

    public void deleteWorkoutExcercise(WorkoutExcercise workoutExcercise)
    {
        throw new NotImplementedException();
    }
}