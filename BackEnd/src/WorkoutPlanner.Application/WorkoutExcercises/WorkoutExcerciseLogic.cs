using WorkoutPlanner.Application.Interfaces.Repositories;
using WorkoutPlanner.Domain;

namespace WorkoutPlanner.Application.WorkoutExcercises;

public class WorkoutExcerciseLogic(IWorkoutExcerciseRepository workoutExcerciseRepository): IWorkoutExcerciseLogic
{
    private readonly IWorkoutExcerciseRepository _workoutExcerciseRepository = workoutExcerciseRepository  ?? throw new ArgumentNullException(nameof(workoutExcerciseRepository));
    public async Task <WorkoutExcercise> createWorkoutExcercise( Guid workoutId, Guid excerciseId, int reps, int sets,
        Enums.LoadType loadType, int? weight = null, int? percentage = null)
    {
        WorkoutExcercise.Validate(sets, reps, loadType, weight, percentage);

        var workoutExcercise= new WorkoutExcercise(sets, reps, loadType, weight, percentage)
        {
            WorkoutId = workoutId,
            ExcerciseId = excerciseId
        };

        await _workoutExcerciseRepository.InsertAsync(workoutExcercise);
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

        _workoutExcerciseRepository.Update(workoutExcercise);
        return workoutExcercise;
    }

    public void deleteWorkoutExcercise(WorkoutExcercise workoutExcercise)
    {
        if (workoutExcercise == null)
        {
            throw new ArgumentException("WorkoutExcercise cannot be null.");
        }
        _workoutExcerciseRepository.Delete(workoutExcercise);
    }

    public async Task<WorkoutExcercise> getWorkoutExcerciseById(Guid id)
    {
        var workoutExcercise = await _workoutExcerciseRepository.GetAsync(we => we.Id == id);
        if (workoutExcercise == null)
        {
            throw new ArgumentNullException(nameof(workoutExcercise), "WorkoutExcercise cannot be null.");
        }

        return workoutExcercise;
    }

    public async Task<IList<WorkoutExcercise>> getAllWorkoutExcercises()
    {
        return await _workoutExcerciseRepository.GetAllAsync();
    }
}