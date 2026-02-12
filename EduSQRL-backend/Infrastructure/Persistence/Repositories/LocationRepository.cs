
using Application.Modules.Locations;
using Domain.Models;
using Infrastructure.Persistence.Data;
using Infrastructure.Persistence.Entities;

namespace Infrastructure.Persistence.Repositories;

public class LocationRepository(EduSqrlDbContext context) : EfcBaseRepository<LocationEntity, Guid, Location>(context), ILocationRepository
{
    public override Task AddAsync(Location model, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public override Location ToModel(LocationEntity entity)
    {
        throw new NotImplementedException();
    }

    public override Task UpdateAsync(Location model, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }
}
