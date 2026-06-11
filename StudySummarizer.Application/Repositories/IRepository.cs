using System.Linq.Expressions;

namespace StudySummarizer.Application.Repositories;

public interface IRepository<T> where T : class
{
    IEnumerable<T> GetAll();
    T? Get(Expression<Func<T, bool>> predicate);
    IEnumerable<T> Find(Expression<Func<T, bool>> predicate);
    void Add(T entity);
    void Update(T entity);
    void Delete(T entity);
}
