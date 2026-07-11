using ECommerce.Domain.Entities;

namespace ECommerce.Domain.DataSeeding;

public interface IGenericRepository<TEnitiy, Tkey> where TEnitiy : BaseEntity<Tkey>
{
    //5 operations for CRUD
    void Add(TEnitiy entity);
    void Update(TEnitiy entity);
    void Delete(TEnitiy entity);
    Task<IReadOnlyList<TEnitiy>> GetAllAsync(CancellationToken ct = default);
    Task<TEnitiy?> GetByIdAsync(Tkey id, CancellationToken ct = default);

}
