using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using WorkoutPlanner.Application.Interfaces.Repositories;
using WorkoutPlanner.Domain;
using WorkoutPlanner.Infraestructure.Persistence;

namespace WorkoutPlanner.Infraestructure.Repositories;

public class WorkoutExcerciseRepository(AppDbContext context)
    : GenericRepository<WorkoutExcercise>(context), IWorkoutExcerciseRepository
{
    public async Task<WorkoutExcercise?> GetWorkoutExcerciseWithIncludesdAsync(Expression<Func<WorkoutExcercise, bool>> predicate)
    {
        return await Context.WorkoutExcercises
            .Include(we => we.Workout)
            .Include(we => we.Excercise)
            .FirstOrDefaultAsync(predicate);
    }

    public async Task<IList<WorkoutExcercise>> GetAllWorkoutExcerciseWithIncludesAsync(Expression<Func<WorkoutExcercise, bool>>? predicate = null)
    {
        IQueryable<WorkoutExcercise> query = Context.WorkoutExcercises
            .Include(we => we.Workout)
            .Include(we => we.Excercise);

        if (predicate != null)
        {
            query = query.Where(predicate);
        }

        return await query.ToListAsync();
    }

    public async Task<IList<WorkoutExcercise>> GetAllByWorkoutIdAsync(Guid workoutId)
    {
        return await Context.WorkoutExcercises
            .Include(we => we.Excercise)
            .Where(we => we.WorkoutId == workoutId)
            .ToListAsync();
    
    }
}