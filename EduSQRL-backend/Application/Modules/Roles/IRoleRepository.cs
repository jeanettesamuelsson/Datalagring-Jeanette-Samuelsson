using Application.Abstractions.Persistence;
using Domain.Models;


namespace Application.Modules.Roles;

public interface IRoleRepository : IBaseRepository<Role, Guid>
{
    
}
