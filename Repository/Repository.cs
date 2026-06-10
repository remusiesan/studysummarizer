using Microsoft.EntityFrameworkCore;
using StudySummarizer.Application.Repositories;
using StudySummarizer.Data;
using System.Linq.Expressions;

namespace StudySummarizer.Repository;

public class Repository<T>(AppDbContext context) : IRepository<T> where T : class
{
    protected readonly AppDbContext _context = context;
    protected readonly DbSet<T> _dbSet = context.Set<T>();

    public IEnumerable<T> GetAll() => _dbSet.AsEnumerable();

    public T? Get(Expression<Func<T, bool>> predicate) => _dbSet.FirstOrDefault(predicate);

    public IEnumerable<T> Find(Expression<Func<T, bool>> predicate) => _dbSet.Where(predicate).AsEnumerable();

    public void Add(T entity)
    {
        ArgumentNullException.ThrowIfNull(entity);
        _dbSet.Add(entity);
    }

    public void Update(T entity)
    {
        ArgumentNullException.ThrowIfNull(entity);
        _dbSet.Update(entity);
    }

    public void Delete(T entity)
    {
        ArgumentNullException.ThrowIfNull(entity);
        _dbSet.Remove(entity);
    }
}
