using ECommerce.Domain.DataSeeding;
using ECommerce.Domain.Entities;
using ECommerce.Infrastructure.Data;

namespace ECommerce.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext dbContext;

    public UnitOfWork(AppDbContext dbContext)
    {
        this.dbContext = dbContext;
    }
    private readonly Dictionary<string, object> repositories = [];
    public IGenericRepository<TEntity, Tkey> GetRepository<TEntity, Tkey>() where TEntity : BaseEntity<Tkey>
    {
        var typeName = typeof(TEntity).Name;
        if (repositories.TryGetValue(typeName, out object? value))
            return (IGenericRepository < TEntity, Tkey >) value;
        // Must cast to IGenericRepository<TEntity, Tkey> to avoid type mismatch
        else
        {
            var repository = new GenericRepository<TEntity, Tkey>(dbContext);
            repositories.Add(typeName, repository);
            return repository;
        }
    }

    public async Task<int> SaveChangesAsync(CancellationToken ct = default)
    => await dbContext.SaveChangesAsync(ct);
}
