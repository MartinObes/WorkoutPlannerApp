using System.Linq.Expressions;

namespace WorkoutPlanner.Application.Interfaces.Repositories;

public interface IGenericRepository<T> where T : class
{
    void Insert(T entity);
    void Update(T entity);
    void Delete(T entity);
    IList<T> GetAll(Expression<Func<T, bool>>? predicate = null, List<string>? includes = null);
    bool Exists(Expression<Func<T, bool>> predicate);
    T Get(Expression<Func<T, bool>> predicate);
    bool CheckConnection();
}