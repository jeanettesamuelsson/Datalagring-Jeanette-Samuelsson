
using Domain.Models;
using Domain.Persistence;

namespace Domain.RepositoryInterfaces;

public interface ICourseRepository : IBaseRepository<Course, Guid>
{
    //method to check that course does not already exist
    Task<bool> CourseAlreadyExistsAsync(string course, CancellationToken ct = default);

    //method to get all courses with dapper
    Task<IReadOnlyList<Course>> GetAllWithDapperAsync(CancellationToken ct = default);
}