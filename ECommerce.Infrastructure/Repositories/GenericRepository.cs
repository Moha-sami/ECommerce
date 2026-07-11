using ECommerce.Domain.DataSeeding;
using ECommerce.Domain.Entities;
using ECommerce.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Infrastructure.Repositories;

public class GenericRepository<TEntity, Tkey> : IGenericRepository<TEntity, Tkey> where TEntity : BaseEntity<Tkey>
{
    private readonly AppDbContext dbContext;

    public GenericRepository(AppDbContext dbContext)
    {
        this.dbContext = dbContext;
    }
    public void Add(TEntity entity)=> dbContext.Add(entity);
    

    public void Delete(TEntity entity) => dbContext.Remove(entity);


    public async Task<IReadOnlyList<TEntity>> GetAllAsync(CancellationToken ct = default)
    => await dbContext.Set<TEntity>().ToListAsync(ct);

    public async Task<TEntity?> GetByIdAsync(Tkey id, CancellationToken ct = default)
    => await dbContext.Set<TEntity>().FindAsync(id, ct);

    public void Update(TEntity entity)=>dbContext.Update(entity);
   
}
