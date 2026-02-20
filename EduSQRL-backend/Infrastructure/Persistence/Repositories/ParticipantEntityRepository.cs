

using Application.Modules.Participants;
using Domain.Participants.ValueObjects;
using Infrastructure.Persistence.Data;
using Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Domain.Models;
using Domain.RepositoryInterfaces;

namespace Infrastructure.Persistence.Repositories;

public class ParticipantEntityRepository(EduSqrlDbContext context) : EfcBaseRepository<ParticipantEntity, Guid, Participant>(context), IParticipantRepository
{
    //method to create a new participant, mapping from ParticipantModel to ParticipantEntity
    public override async Task AddAsync(Participant model, CancellationToken ct = default)
    {
        if (model.Id == Guid.Empty)
            throw new ArgumentException("Id must be set when adding a new participant.");

        
        // add a ToEntity method to map from Model to Entity?
        var entity = new ParticipantEntity
        {
            Id = model.Id,
            Email = model.Email,
            FirstName = model.FirstName,
            LastName = model.LastName,
            PhoneNumber = model.PhoneNumber.Value,
            RoleId = model.RoleId,
            Created = model.Created == default ? DateTime.UtcNow : model.Created,  //set time if not already set
            Concurrency = model.RowVersion
        };

        // EF adds entity to Change Tracker (before repo calls uow to save changes)
        await Set.AddAsync(entity, ct);
    }

    //method to map from ParticipantEntity to ParticipantModel
    public override Participant ToModel(ParticipantEntity entity) => new(

        entity.Id,
        entity.FirstName,
        entity.LastName,
        entity.Email,
        new PhoneNumber(entity.PhoneNumber),
        entity.RoleId,
        entity.Role.RoleName,
        entity.Created,
        entity.Concurrency
        
    );

    public override async Task UpdateAsync(Participant model, CancellationToken ct = default)
    {
        // get the existing entity from database and include roles from ParticipantRoles

        var entity = await Set
            .Include(x => x.Role)
            .SingleOrDefaultAsync(x => x.Id == model.Id, ct)
            ?? throw new ArgumentException($"Participant with id {model.Id} not found.");

        // optimistic concurrency control - set the original value of the concurrency

        Context.Entry(entity).Property(x => x.Concurrency).OriginalValue = model.RowVersion;


        entity.FirstName = model.FirstName;
        entity.LastName = model.LastName;
        entity.Email = model.Email;
        entity.PhoneNumber = model.PhoneNumber.Value;
        entity.RoleId = model.RoleId;
        entity.Created = model.Created;
        entity.Modified = DateTime.UtcNow;

    }


    public async Task<bool> EmailAlreadyExistsAsync(string email, CancellationToken ct = default)
    {
        var normalized = email.Trim();

        return await Set.AsNoTracking().AnyAsync(x => x.Email == normalized, ct);
    }

    
    public override async Task<Participant?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var entity = await Set
            .Include(p => p.Role) 
            .AsNoTracking()
            .SingleOrDefaultAsync(x => x.Id == id, ct);

        return entity is null ? null : ToModel(entity);
    }

    
    public override async Task<IReadOnlyList<Participant>> ListAsync(CancellationToken ct = default)
    {
        var entities = await Set
            .Include(p => p.Role) 
            .AsNoTracking()
            .ToListAsync(ct);

        return entities.Select(ToModel).ToList();
    }
}




