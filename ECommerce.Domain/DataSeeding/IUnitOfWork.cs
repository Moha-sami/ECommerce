using ECommerce.Domain.Entities;

namespace ECommerce.Domain.DataSeeding;

public interface IUnitOfWork
{
    Task<int> SaveChangesAsync(CancellationToken ct = default);

    IGenericRepository<TEntity,Tkey> GetRepository<TEntity,Tkey>() where TEntity : BaseEntity<Tkey>;
}
