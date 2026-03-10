using System.Linq.Expressions;
using WorkoutPlanner.Domain;

namespace WorkoutPlanner.Application.Interfaces.Repositories;

public interface IEvaluationRepository : IGenericRepository<Evaluation>
{
    Task<Evaluation?> GetEvaluationWithIncludesAsync(Expression<Func<Evaluation, bool>> predicate);
    Task<IList<Evaluation>> GetAllEvaluationsWithIncludesAsync(Expression<Func<Evaluation, bool>>? predicate = null);
}