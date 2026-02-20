
using Application.Modules.Locations;
using Domain.Models;
using Domain.RepositoryInterfaces;
using Infrastructure.Persistence.Data;
using Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class LocationRepository(EduSqrlDbContext context) : EfcBaseRepository<LocationEntity, Guid, Location>(context), ILocationRepository
{
    public override async Task AddAsync(Location model, CancellationToken ct = default)
    {
        
        // add a ToEntity method to map from Model to Entity?
        var entity = new LocationEntity
        {
            Id = model.Id,
            Name = model.Name,
            Concurrency = model.RowVersion,
            Created = DateTime.UtcNow,
            Modified = DateTime.UtcNow,

        };

        await Set.AddAsync(entity, ct);
    }

    public override Location ToModel(LocationEntity entity) => new(

        entity.Id,
        entity.Name,
        entity.Concurrency

        );
    

    public override async Task UpdateAsync(Location model, CancellationToken ct = default)
    {
        // get the existing entity from database 

        var entity = await Set
           .SingleOrDefaultAsync(x => x.Id == model.Id, ct)
            ?? throw new ArgumentException($"Location with id {model.Id} not found.");

        // optimistic concurrency control - set the original value of the concurrency

        Context.Entry(entity).Property(x => x.Concurrency).OriginalValue = model.RowVersion;


        entity.Name = model.Name;
        entity.Modified = DateTime.UtcNow;

    }
}
