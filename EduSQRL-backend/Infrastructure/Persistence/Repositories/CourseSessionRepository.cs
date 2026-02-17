
using Application.Modules.Courses;
using Application.Modules.CourseSessions;
using Domain.Models;
using Infrastructure.Persistence.Data;
using Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class CourseSessionRepository(EduSqrlDbContext context) : EfcBaseRepository<CourseSessionEntity, Guid, CourseSession>(context), ICourseSessionRepository
{
    public override async Task AddAsync(CourseSession model, CancellationToken ct = default)
    {
        var entity = new CourseSessionEntity
        {
            Id = model.Id,
            CourseId = model.CourseId,
            LocationId = model.LocationId,
            StartDate = model.StartDate,
            EndDate = model.EndDate,
            Capacity = model.Capacity,
            Concurrency = model.RowVersion,
            Created = DateTime.UtcNow,
            Modified = DateTime.UtcNow

        };

        await Set.AddAsync(entity, ct);
    }

    // map entity to model
    public override CourseSession ToModel(CourseSessionEntity entity) => new(
        
            entity.Id,
            entity.CourseId,
            entity.LocationId,
            entity.Course.CourseName,
            entity.Location.Name, 
            entity.StartDate,
            entity.EndDate,
            entity.Capacity,
            entity.Concurrency

    );
    

    public override async Task UpdateAsync(CourseSession model, CancellationToken ct = default)
    {
        var entity = await Set.SingleOrDefaultAsync(x => x.Id == model.Id, ct)
            ?? throw new ArgumentException($"Session with id { model.Id } not found.");

        // optimistic concurrency control - set the original value of the concurrency

        Context.Entry(entity).Property(x => x.Concurrency).OriginalValue = model.RowVersion;

        // mapping

        entity.CourseId = model.CourseId;
        entity.LocationId = model.LocationId;
        entity.StartDate = model.StartDate;
        entity.EndDate = model.EndDate;
        entity.Capacity = model.Capacity;
        entity.Modified = DateTime.UtcNow;


    }
    public override async Task<IReadOnlyList<CourseSession>> ListAsync(CancellationToken ct = default)
    {
        var entities = await Set
            .Include(x => x.Course)
            .Include(x => x.Location)
            .AsNoTracking()
            .ToListAsync(ct);

        return entities.Select(ToModel).ToList();
    }
}
