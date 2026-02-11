using Application.Modules.PersistanceModels;
using Application.Modules.Roles.Outputs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Modules.Roles;

public interface IRoleService
{
    // get all roles
    Task<IEnumerable<RoleOutput>> GetRolesAsync(CancellationToken ct = default);

}
