using Domain.Models;
using Application.Modules.Roles.Outputs;


namespace Application.Modules.Roles;

public interface IRoleService
{
    // get all roles
    Task<IEnumerable<RoleOutput>> GetRolesAsync(CancellationToken ct = default);

}
