using System.Linq.Expressions;
using WorkoutPlanner.Domain;

namespace WorkoutPlanner.Application.Interfaces.Repositories;

public interface IWorkoutExcerciseRepository : IGenericRepository<WorkoutExcercise>
{
    Task<WorkoutExcercise?> GetWorkoutExcerciseWithIncludesdAsync(Expression<Func<WorkoutExcercise, bool>> predicate);
    Task<IList<WorkoutExcercise>> GetAllWorkoutExcerciseWithIncludesAsync(Expression<Func<WorkoutExcercise, bool>>? predicate = null);
    Task<IList<WorkoutExcercise>> GetAllByWorkoutIdAsync(Guid workoutId);
}