
using Application.Modules.Courses;
using Application.Modules.CourseSessions;
using Domain.Models;
using Infrastructure.Persistence.Data;
using Infrastructure.Persistence.Entities;

namespace Infrastructure.Persistence.Repositories;

public class CourseSessionRepository(EduSqrlDbContext context) : EfcBaseRepository<CourseSessionEntity, Guid, CourseSession>(context), ICourseSessionRepository
{
    public override Task AddAsync(CourseSession model, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public override CourseSession ToModel(CourseSessionEntity entity)
    {
        throw new NotImplementedException();
    }

    public override Task UpdateAsync(CourseSession model, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }
}
