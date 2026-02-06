
namespace Application.Abstractions.Persistence;

public interface IBaseRepository<TModel, in TKey>
{
    //create 
    Task AddAsync(TModel model, CancellationToken ct = default);

    //read (all and by id)
    Task<TModel?> GetByIdAsync(TKey id, CancellationToken ct = default);

    Task<IReadOnlyList<TModel>> ListAsync(CancellationToken ct = default);

    // update
    Task UpdateAsync(TModel model, CancellationToken ct = default);

   // delete
    Task DeleteAsync(TKey id, CancellationToken ct = default);

   
}


