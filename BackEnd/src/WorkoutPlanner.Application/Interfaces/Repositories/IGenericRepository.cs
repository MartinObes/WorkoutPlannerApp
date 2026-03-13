using System.Linq.Expressions;

namespace WorkoutPlanner.Application.Interfaces.Repositories;

public interface IGenericRepository<T> where T : class
{
    Task InsertAsync(T entity);
    void Update(T entity);
    void Delete(T entity);
    Task<IList<T>> GetAllAsync(Expression<Func<T, bool>>? predicate = null, List<string>? includes = null);
    Task<T?> GetAsync(Expression<Func<T, bool>> predicate, List<string>? includes = null);
    Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate);
    Task SaveAsync();
    Task<bool> CheckConnectionAsync();
}