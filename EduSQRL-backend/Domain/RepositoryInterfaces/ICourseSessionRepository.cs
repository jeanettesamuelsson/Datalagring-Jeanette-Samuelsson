
using Domain.Models;
using Domain.Persistence;

namespace Domain.RepositoryInterfaces;

public interface ICourseSessionRepository : IBaseRepository<CourseSession, Guid>   
{
}
