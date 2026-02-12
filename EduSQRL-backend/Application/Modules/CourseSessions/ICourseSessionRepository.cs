using Application.Abstractions.Persistence;
using Domain.Models;


namespace Application.Modules.CourseSessions;

public interface ICourseSessionRepository : IBaseRepository<CourseSession, Guid>   
{
}
