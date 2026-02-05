
using Application.Abstractions.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Infrastructure.Persistence.Repositories;

public abstract class EfcBaseRepository<TEntity, TKey, TModel>(DbContext context) : IBaseRepository<TModel, TKey> where TEntity : class, IEntity<TKey>
{
    // context / conection to database
    protected DbContext Context { get; } = context;
    protected DbSet<TEntity> Set => Context.Set<TEntity>();


    public abstract TModel ToModel(TEntity entity);

    // Add
    public abstract Task<TModel> AddAsync(TModel entity, CancellationToken ct = default);

    // Update
    public abstract Task<bool> UpdateAsync(TModel entity, CancellationToken ct = default);
    
     
    // list all 
    public async Task<IReadOnlyList<TModel>> ListAsync(CancellationToken ct = default)
    {
        var entities = await Set.AsNoTracking().ToListAsync(ct);

        return [..entities.Select(ToModel)];
    }

    // get by id
    public virtual async Task<TModel?> GetByIdAsync(TKey id, CancellationToken ct = default)
    {
        var entity = await Set.AsNoTracking().SingleOrDefaultAsync(x => x.Id!.Equals(id), ct);
        
        return entity is null ? default : ToModel(entity);

    }

    // delete
    public virtual async Task DeleteAsync(TKey id, CancellationToken ct = default)
    {
        var entity = await Set.SingleOrDefaultAsync(x => x.Id!.Equals(id), ct);

        if (entity is null) return;

        Set.Remove(entity);
    }
}
