
using Application.Abstractions.Persistence;
using Application.Modules.Locations.Input;
using Application.Modules.Locations.Output;
using Application.Modules.Participants.Inputs;
using Application.Modules.Participants.Outputs;
using Application.Modules.Roles;
using Domain.Models;
using Domain.Participants.ValueObjects;

namespace Application.Modules.Locations;

public class LocationService

    (
    ILocationRepository locations,
    IUnitOfWork uow
    ) : ILocationService

{

    private static LocationOutput ToOutputModel(Location p) => new(

        p.Id,
        p.Name, 
        p.RowVersion

        );

    // create 
    public async Task<Guid> CreateAsync(CreateLocationInput input, CancellationToken ct)
    {
       
        var locationId = Guid.NewGuid();
        var dateNow = DateTime.UtcNow;

        var location = new Location(
            Id: locationId, 
            Name: input.Name,
            RowVersion: []

            );

        await locations.AddAsync(location);

        await uow.SaveChangesAsync(ct);

        return locationId;

    }

    // read (all and by id)
    public async Task<IReadOnlyList<LocationOutput>> GetAllAsync(CancellationToken ct = default)
    {
        var list = await locations.ListAsync(ct);

        return [.. list.Select(ToOutputModel)];
    }

    public async Task<LocationOutput?> GetByIdAsync(Guid locationId, CancellationToken ct)
    {
        var location = await locations.GetByIdAsync(locationId, ct);

        return location is null ? null : ToOutputModel(location);
    }


    //update
    public async Task<LocationOutput?> UpdateAsync(UpdateLocationInput input, CancellationToken ct)
    {

        var location = await locations.GetByIdAsync(input.Id, ct);
        if (location is null)
            return null;


        var updatedLocation = location with
        {
            Name = input.Name,
            RowVersion = input.RowVersion
        };

        // update and save changes

        await locations.UpdateAsync(updatedLocation, ct);
        await uow.SaveChangesAsync(ct);

        // return the updated location

        var updated = await locations.GetByIdAsync(input.Id, ct);
        return updated is null ? null : ToOutputModel(updated);


    }

    //delete
    public async Task DeleteAsync(Guid locationId, byte[] rowVersion, CancellationToken ct = default)
    {
        var location = await locations.GetByIdAsync(locationId, ct)
                       ?? throw new ArgumentException("Location not found");

        await locations.UpdateAsync(location with { RowVersion = rowVersion }, ct);

        await locations.DeleteAsync(locationId, ct);

        await uow.SaveChangesAsync(ct);
    }


}
