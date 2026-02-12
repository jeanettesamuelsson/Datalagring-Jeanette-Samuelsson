
using Application.Modules.CourseSessions.Input;
using Application.Modules.CourseSessions.Output;


namespace Application.Modules.CourseSessions;

public interface ICourseSessionService
{
    // create
    Task<Guid> CreateAsync(CreateCourseSessionInput input, CancellationToken cancellationToken);

    // delete
    Task DeleteAsync(Guid CourseSessionId, byte[] rowVersion, CancellationToken cancellationToken);

    // get all 
    Task<IReadOnlyList<CourseSessionOutput>> GetAllCourseSessionsAsync(CancellationToken cancellationToken);

    // get by ID
    Task<CourseSessionOutput?> GetByIdAsync(Guid id, CancellationToken cancellationToken);

    // update
    Task<CourseSessionOutput?> UpdateAsync(UpdateCourseSessionInput input, CancellationToken cancellationToken);


}
