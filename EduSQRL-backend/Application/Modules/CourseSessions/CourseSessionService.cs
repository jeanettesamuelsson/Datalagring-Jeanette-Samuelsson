using Application.Abstractions.Persistence;
using Application.Modules.Courses;
using Application.Modules.CourseSessions.Input;
using Application.Modules.CourseSessions.Output;
using Application.Modules.Locations;
using Domain.Models;


namespace Application.Modules.CourseSessions;

public class CourseSessionService(
    
    ICourseSessionRepository courseSessions,
    ICourseRepository courses,
    ILocationRepository locations,
    IUnitOfWork uow
    ) : ICourseSessionService

{

    // map from Domain to output
    private static CourseSessionOutput ToOutputModel(CourseSession c) => new(

        c.Id,
        c.CourseId,
        c.LocationId,
        c.CourseName,
        c.LocationName,
         c.StartDate,
        c.EndDate,
        c.Capacity,
        c.RowVersion

        );

    // create 
    public async Task<Guid> CreateAsync(CreateCourseSessionInput input, CancellationToken ct)
    {
        // checks for location and course before creating

        var course = await courses.GetByIdAsync(input.CourseId, ct)
            ?? throw new ArgumentException($"Course with ID {input.CourseId} does not exist");

        var location = await locations.GetByIdAsync (input.LocationId, ct) 
            ?? throw new ArgumentException($"Location with ID {input.LocationId} does not exist");


        var courseSessionId = Guid.NewGuid();

        var courseSession = new CourseSession(

            Id: courseSessionId,
            CourseId: input.CourseId,
            LocationId: input.LocationId,
            CourseName: input.CourseName,
            LocationName: input.LocationName,
            StartDate: input.StartDate,
            EndDate: input.EndDate,
            Capacity: input.Capacity,
            RowVersion: []

            );

        await courseSessions.AddAsync(courseSession, ct);

        await uow.SaveChangesAsync(ct);

        return courseSessionId;

    }

    // read (all and by id)
    public async Task<IReadOnlyList<CourseSessionOutput>> GetAllCourseSessionsAsync(CancellationToken ct = default)
    {
        var list = await courseSessions.ListAsync(ct);

        return [.. list.Select(ToOutputModel)];
    }

    public async Task<CourseSessionOutput?> GetByIdAsync(Guid courseSessionId, CancellationToken ct)
    {
        var courseSession = await courseSessions.GetByIdAsync(courseSessionId, ct);

        return courseSession is null ? null : ToOutputModel(courseSession);
    }


    //update
    public async Task<CourseSessionOutput?> UpdateAsync(UpdateCourseSessionInput input, CancellationToken ct)
    {
        var courseSession = await courseSessions.GetByIdAsync(input.Id, ct);
        if (courseSession is null)
            return null;

        // mapper

        var updatedCourse = courseSession with
        {
            LocationId = input.LocationId,
            StartDate = input.StartDate,
            EndDate = input.EndDate,
            Capacity = input.Capacity,
            RowVersion = input.RowVersion

        };

        await courseSessions.UpdateAsync(updatedCourse, ct);
        await uow.SaveChangesAsync(ct);

        // get updated and return

        var result = await courseSessions.GetByIdAsync(input.Id, ct);
        return result is null ? null : ToOutputModel(result);


    }



    //delete
    public async Task DeleteAsync(Guid courseSessionId, byte[] rowVersion, CancellationToken ct = default)
    {
        var courseSession= await courseSessions.GetByIdAsync(courseSessionId, ct)
                       ?? throw new ArgumentException("Course session not found");

        await courseSessions.DeleteAsync(courseSessionId, ct);

        await uow.SaveChangesAsync(ct);
    }


}


