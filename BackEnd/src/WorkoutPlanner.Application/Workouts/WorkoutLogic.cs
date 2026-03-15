using WorkoutPlanner.Application.Interfaces.Repositories;
using WorkoutPlanner.Application.WorkoutExcercises;
using WorkoutPlanner.Domain;
using WorkoutPlanner.Domain.AuxiliaryDomainClasses;

namespace WorkoutPlanner.Application.Workouts;

public class WorkoutLogic(IWorkoutRepository workoutRepository, IWorkoutExcerciseLogic workoutExcerciseLogic) : IWorkoutLogic
{
    private readonly IWorkoutRepository _workoutRepository = workoutRepository ?? throw new ArgumentNullException(nameof(workoutRepository));
    private readonly IWorkoutExcerciseLogic _workoutExcerciseLogic = workoutExcerciseLogic ?? throw new ArgumentNullException(nameof(workoutExcerciseLogic));

    public async Task<Workout> CreateWorkout(string name, Guid? coachId, IList<CreateWorkoutExcerciseArgs> workoutExcerciseArgsList)
    {
        var normalizedName = NormalizeName(name);
        var workout = new Workout
        {
            Id = Guid.NewGuid(),
            Name = normalizedName,
            CoachId = coachId
        };
        
        foreach (var wEArgs in workoutExcerciseArgsList)
        {
            var workoutExcercise = await _workoutExcerciseLogic.createWorkoutExcercise(workout.Id, wEArgs.ExcerciseId, wEArgs.Reps,
                wEArgs.Sets, wEArgs.LoadType, wEArgs.Weight, wEArgs.Percentage);
            workout.WorkoutExcercises.Add(workoutExcercise);
        }

        await _workoutRepository.InsertAsync(workout);
        return workout;
    }

    public Workout UpdateWorkout(Workout workout, string name, Guid? coachId)
    {
        if (workout == null) throw new ArgumentException("Workout cannot be null.");

        var normalizedName = NormalizeName(name);
        workout.Name = normalizedName;
        workout.CoachId = coachId;

        _workoutRepository.Update(workout);
        return workout;
    }

    public void DeleteWorkout(Workout workout)
    {
        if (workout == null)
        {
            throw new ArgumentNullException(nameof(workout), "Workout cannot be null.");
        }

        _workoutRepository.Delete(workout);
    }

    public async Task<bool> WorkoutExists(string name)
    {
        var normalizedName = NormalizeName(name);
        return await _workoutRepository.ExistsAsync(w => w.Name == normalizedName);
    }

    public async Task<Workout> GetWorkoutByName(string name)
    {
        var normalizedName = NormalizeName(name);
        var workout = await _workoutRepository.GetAsync(w => w.Name == normalizedName);
        if (workout == null)
        {
            throw new ArgumentNullException(nameof(workout), "Workout cannot be null.");
        }

        return workout;
    }

    public async Task<IList<Workout>> GetAllWorkouts()
    {
        var workouts = await _workoutRepository.GetAllAsync();
        return workouts.OrderBy(w => w.Name).ToList();
    }

    private static string NormalizeName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Name cannot be empty.", nameof(name));
        }
        
        return name.Trim();
    }
}