using Application.Modules.Courses.Input;
using Application.Modules.Courses.Output;


namespace Application.Modules.Courses;

public interface ICourseService
{

    // create
    Task<Guid> CreateAsync(CreateCourseInput input, CancellationToken cancellationToken);

    // delete
    Task DeleteAsync(Guid CourseId, byte[] rowVersion, CancellationToken cancellationToken);

    // get all 
    Task<IReadOnlyList<CourseOutput>> GetAllCoursesAsync(CancellationToken cancellationToken);

    // get by ID
    Task<CourseOutput?> GetByIdAsync(Guid id, CancellationToken cancellationToken);

    // update
    Task<CourseOutput?> UpdateAsync(UpdateCourseInput input, CancellationToken cancellationToken);

}
