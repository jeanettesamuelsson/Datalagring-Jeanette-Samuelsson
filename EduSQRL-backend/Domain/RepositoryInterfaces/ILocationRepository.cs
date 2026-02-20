
using Domain.Models;
using Domain.Persistence;

namespace Domain.RepositoryInterfaces;

public interface ILocationRepository : IBaseRepository<Location, Guid>
{
}
