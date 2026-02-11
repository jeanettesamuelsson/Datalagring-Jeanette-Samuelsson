using Application.Abstractions.Persistence;
using Application.Modules.PersistanceModels;


namespace Application.Modules.Roles;

public interface IRoleRepository : IBaseRepository<Role, Guid>
{
    
}
