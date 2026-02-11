using Application.Abstractions.Persistence;
using Infrastructure.Persistence.Data;

namespace Infrastructure.Persistence.UnitOfWork;

public class EfcUnitOfWork(EduSqrlDbContext context) : IUnitOfWork
{
    public Task<int> SaveChangesAsync(CancellationToken ct = default) => context.SaveChangesAsync(ct);

}
