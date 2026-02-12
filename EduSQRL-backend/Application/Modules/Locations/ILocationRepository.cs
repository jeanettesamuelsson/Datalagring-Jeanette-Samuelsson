using Application.Abstractions.Persistence;
using Domain.Models;



namespace Application.Modules.Locations;

public interface ILocationRepository : IBaseRepository<Location, Guid>
{
}
