
using Application.Modules.Courses.Input;
using Application.Modules.Courses.Output;
using Application.Modules.Locations.Input;
using Application.Modules.Locations.Output;
using Application.Modules.Participants.Inputs;
using Application.Modules.Participants.Outputs;

namespace Application.Modules.Locations;

public interface ILocationService 
{
    // create
    Task<Guid> CreateAsync(CreateLocationInput input, CancellationToken cancellationToken);

    // delete
    Task DeleteAsync(Guid locationId, byte[] rowVersion, CancellationToken cancellationToken);

    // get all 
    Task<IReadOnlyList<LocationOutput>> GetAllAsync(CancellationToken cancellationToken);

    // get by ID
    Task<LocationOutput?> GetByIdAsync(Guid id, CancellationToken cancellationToken);

    // update

    Task<LocationOutput?> UpdateAsync(UpdateLocationInput input, CancellationToken cancellationToken);
}
