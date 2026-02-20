
using Application.Modules.Roles.Outputs;
using Domain.RepositoryInterfaces;


namespace Application.Modules.Roles;

public class RoleService(IRoleRepository roles) : IRoleService
{
    private readonly IRoleRepository _roles = roles;

    public async Task<IEnumerable<RoleOutput>> GetRolesAsync(CancellationToken ct = default)
    {
        var roles = await _roles.ListAsync(ct);

        // map to output model

        return roles.Select(r => new RoleOutput(r.Id, r.RoleName));
    }
}
