using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using WorkoutPlanner.Application.Interfaces.Repositories;
using WorkoutPlanner.Domain;
using WorkoutPlanner.Infraestructure.Persistence;

namespace WorkoutPlanner.Infraestructure.Repositories;

public class EvaluationRepository(AppDbContext context)
    : GenericRepository<Evaluation>(context), IEvaluationRepository
{
    public async Task<Evaluation?> GetEvaluationWithIncludesAsync(Expression<Func<Evaluation, bool>> predicate)
    {
        return await Context.Evaluations
            .Include(e => e.Player)
            .FirstOrDefaultAsync(predicate);
    }

    public async Task<IList<Evaluation>> GetAllEvaluationsWithIncludesAsync(Expression<Func<Evaluation, bool>>? predicate = null)
    {
        IQueryable<Evaluation> query = Context.Evaluations
            .Include(e => e.Player);

        if (predicate != null)
        {
            query = query.Where(predicate);
        }

        return await query.ToListAsync();
    }
}