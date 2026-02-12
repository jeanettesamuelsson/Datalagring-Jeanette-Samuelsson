using Application.Modules.Courses;
using Domain.Models;
using Domain.Participants.ValueObjects;
using Infrastructure.Persistence.Data;
using Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;


namespace Infrastructure.Persistence.Repositories;

public class CourseRepository(EduSqrlDbContext context) : EfcBaseRepository<CourseEntity, Guid, Course>(context), ICourseRepository
{
    
    

    public override async Task AddAsync(Course model, CancellationToken ct = default)
    {
        if (model.Id == Guid.Empty)
            throw new ArgumentException("Id must be set when adding a new course.");

      
        // add a ToEntity method to map from Model to Entity?
        var entity = new CourseEntity
        {
            Id = model.Id,
            CourseCode = model.CourseCode,
            CourseName = model.CourseName,
            Description = model.Description,
            Concurrency = model.RowVersion,
            Created = DateTime.UtcNow,
            Modified = DateTime.UtcNow,
            
        };

        await Set.AddAsync(entity, ct);
    }

    //method to map from CourseEntity to CourseModel
    public override Course ToModel(CourseEntity entity) => new(

        entity.Id,
        entity.CourseCode, 
        entity.CourseName,
        entity.Description,
        entity.Concurrency

    );

    public override async Task UpdateAsync(Course model, CancellationToken ct = default)
    {
        // get the existing entity from database 

        var entity = await Set
            .SingleOrDefaultAsync(x => x.Id == model.Id, ct)
            ?? throw new ArgumentException($"Course with id {model.Id} not found.");

        // optimistic concurrency control - set the original value of the concurrency

        Context.Entry(entity).Property(x => x.Concurrency).OriginalValue = model.RowVersion;


        entity.CourseCode = model.CourseCode;
        entity.CourseName = model.CourseName;
        entity.Description = model.Description;
        entity.Modified = DateTime.UtcNow;

    }


    public async Task<bool> CourseAlreadyExistsAsync(string course, CancellationToken ct = default)
    {
        var normalized = course.Trim();

        return await Set.AsNoTracking().AnyAsync(x => x.CourseName == normalized, ct);
    }

    
    public async override Task<Course?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var entity = await Set
     
            .AsNoTracking()
            .SingleOrDefaultAsync(x => x.Id == id, ct);

        return entity is null ? null : ToModel(entity);
    }

   
    public async override Task<IReadOnlyList<Course>> ListAsync(CancellationToken ct = default)
    {
        var entities = await Set
            .AsNoTracking()
            .ToListAsync(ct);

        return entities.Select(ToModel).ToList();
    }

}
