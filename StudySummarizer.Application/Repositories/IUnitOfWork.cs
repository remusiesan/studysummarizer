using StudySummarizer.Domain.Entities;

namespace StudySummarizer.Application.Repositories;

public interface IUnitOfWork : IDisposable
{
    IRepository<User> Users { get; }
    IRepository<Document> Documents { get; }
    IRepository<Summary> Summaries { get; }
    IRepository<AIModel> AIModels { get; }

    Task<int> SaveChangesAsync();
    int SaveChanges();
}
