using Application.Abstractions.Persistence;
using Domain.Models;


namespace Application.Modules.Courses;

public interface ICourseRepository : IBaseRepository<Course, Guid>
{
    //method to check that course does not already exist
    Task<bool> CourseAlreadyExistsAsync(string course, CancellationToken ct = default);


}