
using Application.Modules.Roles;
using Infrastructure.Persistence.Data;
using Infrastructure.Persistence.Entities;
using Domain.Models;


namespace Infrastructure.Persistence.Repositories;

public class RoleRepository(EduSqrlDbContext context) : EfcBaseRepository<RoleEntity, Guid, Role>(context), IRoleRepository
{
    // map RoleEntity -> Role record
    public override Role ToModel(RoleEntity entity) => new(
        entity.Id,
        entity.RoleName
    );

    public override async Task AddAsync(Role model, CancellationToken ct = default)
    {
        throw new NotImplementedException("Adding new roles is not supported.");
    }

    public override Task UpdateAsync(Role model, CancellationToken ct = default)
    {
        throw new NotImplementedException("Updating roles is not supported.");
    }
}

