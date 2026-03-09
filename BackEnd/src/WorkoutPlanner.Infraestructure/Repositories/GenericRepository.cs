using WorkoutPlanner.Application.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using WorkoutPlanner.Infraestructure.Persistence;

namespace WorkoutPlanner.Infraestructure.Repositories;

public class GenericRepository<T>(AppDbContext context) : IGenericRepository<T>
    where T : class
{
    protected AppDbContext Context { get; } = context;
    protected DbSet<T> DbSet => Context.Set<T>();

    public virtual async Task InsertAsync(T entity)
    {
        await DbSet.AddAsync(entity);
    }

    public virtual void Update(T entity)
    {
        DbSet.Update(entity);
    }

    public virtual void Delete(T entity)
    {
        if (Context.Entry(entity).State == EntityState.Detached)
        {
            DbSet.Attach(entity);
        }

        DbSet.Remove(entity);
    }

    public virtual async Task<IList<T>> GetAllAsync(
        Expression<Func<T, bool>>? predicate = null,
        List<string>? includes = null)
    {
        IQueryable<T> query = DbSet;

        if (includes != null)
        {
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
        }

        if (predicate != null)
        {
            query = query.Where(predicate);
        }

        return await query.ToListAsync();
    }

    public virtual async Task<T?> GetAsync(
        Expression<Func<T, bool>> predicate,
        List<string>? includes = null)
    {
        IQueryable<T> query = DbSet;

        if (includes != null)
        {
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
        }

        return await query.FirstOrDefaultAsync(predicate);
    }

    public virtual async Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate)
    {
        return await DbSet.AnyAsync(predicate);
    }

    public virtual async Task SaveAsync()
    {
        await Context.SaveChangesAsync();
    }

    public virtual async Task<bool> CheckConnectionAsync()
    {
        return await Context.Database.CanConnectAsync();
    }
}