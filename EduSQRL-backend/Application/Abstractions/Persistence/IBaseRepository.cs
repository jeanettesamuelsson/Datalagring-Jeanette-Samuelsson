
namespace Application.Abstractions.Persistence;

public interface IBaseRepository<TModel, in TKey>
{
    //create 
    Task<TModel> AddAsync(TModel entity, CancellationToken ct = default);

    //read (all and by id)
    Task<TModel?> GetByIdAsync(TKey id, CancellationToken ct = default);

    Task<IReadOnlyList<TModel>> ListAsync(CancellationToken ct = default);

    // update
    Task<bool> UpdateAsync(TModel entity, CancellationToken ct = default);

   // delete
    Task DeleteAsync(TKey id, CancellationToken ct = default);

   
}


