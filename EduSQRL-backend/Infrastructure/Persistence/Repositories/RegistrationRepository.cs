
using Application.Modules.Registrations;
using Domain.Models;
using Infrastructure.Persistence.Data;
using Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;


namespace Infrastructure.Persistence.Repositories;

public class RegistrationRepository(EduSqrlDbContext context) : EfcBaseRepository<RegistrationEntity, Guid, Registration>(context), IRegistrationRepository
{
    //method to create a new registration, mapping from Model to Entity
    public override async Task AddAsync(Registration model, CancellationToken ct = default)
    {
      
        var entity = new RegistrationEntity
        {
            Id = model.Id,
            ParticipantId = model.ParticipantId,
            CourseSessionId = model.CourseSessionId,
            Status = model.Status,
            Created = model.Created == default ? DateTime.UtcNow : model.Created,  //set time if not already set
            Concurrency = model.RowVersion
        };

        await Set.AddAsync(entity, ct);
    }

    //method to map from Registration entity to Registration model
    public override Registration ToModel(RegistrationEntity entity) => new(

        entity.Id,
        entity.ParticipantId,
        $"{entity.Participant?.FirstName} {entity.Participant?.LastName}",
        entity.CourseSessionId,
        entity.CourseSession?.Course?.CourseName ?? "Unknown",
        entity.Status,
        entity.Created,
        entity.Concurrency

    );

    // update
    public override async Task UpdateAsync(Registration model, CancellationToken ct = default)
    {
        // get the existing entity from database

        var entity = await Set
            .SingleOrDefaultAsync(x => x.Id == model.Id, ct)
            ?? throw new ArgumentException($"Registration with id {model.Id} not found.");

        // optimistic concurrency control - set the original value of the concurrency

        Context.Entry(entity).Property(x => x.Concurrency).OriginalValue = model.RowVersion;


        entity.Status = model.Status;
        entity.Modified = DateTime.UtcNow;

    }

    // get by id
    public override async Task<Registration?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var entity = await Set
            .Include(r => r.Participant)
            .Include(r => r.CourseSession)

            // then include to also get course name connected to that session
            .ThenInclude(s => s.Course)
            .AsNoTracking()
            .SingleOrDefaultAsync(x => x.Id == id, ct);

        return entity is null ? null : ToModel(entity);
    }

    // list all registrations
    public override async Task<IReadOnlyList<Registration>> ListAsync(CancellationToken ct = default)
    {
        var entities = await Set
            .Include(r => r.Participant)
            .Include(r => r.CourseSession)
            .ThenInclude(s => s.Course)
            .AsNoTracking()
            .ToListAsync(ct);

        return entities.Select(ToModel).ToList();
    }

    // method to check if participant is already registered
    public async Task<bool> ExistsAsync(Guid participantId, Guid courseSessionId, CancellationToken ct = default)
    {

        return await Set.AnyAsync(r =>
            r.ParticipantId == participantId &&
            r.CourseSessionId == courseSessionId, ct);

        //returns true if match -> participant id already registered to the session id
    }

}
