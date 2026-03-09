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
    protected AppDbContext Context { get; set; } = context ?? throw new ArgumentNullException(nameof(context));

    public virtual void Insert(T entity)
    {
        if(entity == null)
        {
            throw new ArgumentNullException(nameof(entity), "Entity cannot be null");
        }

        Context.Set<T>().Add(entity);
        Save();
    }

    public virtual void Update(T entity)
    {
        Context.Entry(entity).State = EntityState.Modified;
        Save();
    }

    public virtual void Delete(T entity)
    {
        if(Context.Entry(entity).State == EntityState.Detached)
        {
            Context.Set<T>().Attach(entity);
        }

        Context.Set<T>().Remove(entity);
        Save();
    }

    public virtual IList<T> GetAll(Expression<Func<T, bool>>? predicate = null, List<string>? includes = null)
    {
        IQueryable<T> query = Context.Set<T>();

        if(includes != null)
        {
            foreach(var variable in includes)
            {
                query = query.Include(variable);
            }
        }

        return query.Where(predicate ?? (x => true)).ToList();
    }

    public virtual bool Exists(Expression<Func<T, bool>> predicate)
    {
        return Context.Set<T>().Any(predicate);
    }

    public virtual T? Get(Expression<Func<T, bool>> predicate)
    {
        return Context.Set<T>().FirstOrDefault(predicate);
    }

    private void Save()
    {
        Context.SaveChanges();
    }

    public virtual bool CheckConnection()
    {
        try
        {
            return Context.Database.EnsureCreated();
        }
        catch
        {
            return false;
        }
    }
}