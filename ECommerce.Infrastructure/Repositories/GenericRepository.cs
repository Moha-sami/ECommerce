using ECommerce.Domain.DataSeeding;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Specifications;
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

    public async Task<TEntity?> GetEntityWithSpec(ISpecification<TEntity> spec, CancellationToken ct = default)
    {
        return await ApplySpecification(spec).FirstOrDefaultAsync(ct);
    }

    public async Task<IReadOnlyList<TEntity>> ListWithSpecAsync(ISpecification<TEntity> spec, CancellationToken ct = default)
    {
        return await ApplySpecification(spec).ToListAsync(ct);
    }

    private IQueryable<TEntity> ApplySpecification(ISpecification<TEntity> spec)
    {
        return SpecificationEvaluator<TEntity>.GetQuery(dbContext.Set<TEntity>().AsQueryable(), spec);
    }
}

