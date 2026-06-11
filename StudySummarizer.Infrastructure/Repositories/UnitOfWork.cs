using StudySummarizer.Application.Repositories;
using StudySummarizer.Domain.Entities;
using StudySummarizer.Infrastructure.Data;

namespace StudySummarizer.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;
    private IRepository<User>? _userRepository;
    private IRepository<Document>? _documentRepository;
    private IRepository<Summary>? _summaryRepository;
    private IRepository<AIModel>? _aiModelRepository;

    public UnitOfWork(AppDbContext context)
    {
        _context = context;
    }

    public IRepository<User> Users => _userRepository ??= new Repository<User>(_context);
    public IRepository<Document> Documents => _documentRepository ??= new Repository<Document>(_context);
    public IRepository<Summary> Summaries => _summaryRepository ??= new Repository<Summary>(_context);
    public IRepository<AIModel> AIModels => _aiModelRepository ??= new Repository<AIModel>(_context);

    public async Task<int> SaveChangesAsync() => await _context.SaveChangesAsync();

    public int SaveChanges() => _context.SaveChanges();

    public void Dispose()
    {
        _context?.Dispose();
        GC.SuppressFinalize(this);
    }
}
