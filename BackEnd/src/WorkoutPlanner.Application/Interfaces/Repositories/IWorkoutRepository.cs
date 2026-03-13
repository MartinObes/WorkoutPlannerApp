using System.Linq.Expressions;
using WorkoutPlanner.Domain;

namespace WorkoutPlanner.Application.Interfaces.Repositories;

public interface  IWorkoutRepository : IGenericRepository<Workout>
{
    Task<Workout?> GetWorkoutWithIncludesAsync(Expression<Func<Workout, bool>> predicate);
    Task<IList<Workout>> GetAllWorkoutsWithIncludesAsync(Expression<Func<Workout, bool>>? predicate = null);
}