
using Application.Modules.Participants;
using Application.Modules.Participants.PersistanceModels;
using Infrastructure.Persistence.Entities;
using Infrastructure.Persistence.Migrations.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Infrastructure.Persistence.Repositories;

public class ParticipantEntityRepository(EduSqrlDbContext context) : EfcBaseRepository<ParticipantEntity, Guid, Participant>(context), IParticipantRepository
{
    //method to create a new participant, mapping from ParticipantModel to ParticipantEntity
    public override async Task AddAsync(Participant model, CancellationToken ct = default)
    {
        if (model.Id == Guid.Empty)
            throw new ArgumentException("Id must be set when adding a new participant.");

        var entity = new ParticipantEntity
        {
            Id = model.Id,
            Email = model.Email,
            Created = model.Created == default ? DateTime.UtcNow : model.Created,  //set time if not already set
            Concurrency = model.RowVersion
        };

        await Set.AddAsync(entity, ct);
    }

    //method to map from ParticipantEntity to ParticipantModel
    public override Participant ToModel(ParticipantEntity entity) => new(
        entity.Id,
        entity.Email,
        entity.Created,
        entity.Concurrency

        );
    
    public override async Task UpdateAsync(Participant model, CancellationToken ct = default)
    {
        var entity = await Set.SingleOrDefaultAsync(x => x.Id == model.Id, ct)
            ?? throw new ArgumentException($"Participant with id {model.Id} not found.");

        Context.Entry(entity).Property(x => x.Concurrency).OriginalValue = model.RowVersion;


        entity.Email = model.Email;
        entity.Created = model.Created;
        entity.Modified = DateTime.UtcNow;

    }


    public async Task<bool> EmailAlreadyExistsAsync(string email, CancellationToken ct = default)
    {
        var normalized = email.Trim();

        return await Set.AsNoTracking().AnyAsync(x => x.Email == normalized, ct);
    }

}




