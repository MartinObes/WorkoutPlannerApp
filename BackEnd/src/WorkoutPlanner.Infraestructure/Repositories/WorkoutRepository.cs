using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using WorkoutPlanner.Application.Interfaces.Repositories;
using WorkoutPlanner.Domain;
using WorkoutPlanner.Infraestructure.Persistence;

namespace WorkoutPlanner.Infraestructure.Repositories;

public class WorkoutRepository(AppDbContext context) : GenericRepository<Workout>(context), IWorkoutRepository
{
    public async Task<Workout?> GetWorkoutWithIncludesAsync(Expression<Func<Workout, bool>> predicate)
    {
        return await Context.Workouts
            .Include(w => w.Coach)
            .Include(w => w.WorkoutExcercises)
            .ThenInclude(we => we.Excercise)
            .FirstOrDefaultAsync(predicate);
    }

    public async Task<IList<Workout>> GetAllWorkoutsWithIncludesAsync(Expression<Func<Workout, bool>>? predicate = null)
    {
        var query = Context.Workouts
            .Include(w => w.Coach)
            .Include(w => w.WorkoutExcercises)
            .ThenInclude(we => we.Excercise)
            .AsQueryable();

        if (predicate != null)
        {
            query = System.Linq.Queryable.Where(query, predicate);
        }

        return await query.ToListAsync();
    }
}