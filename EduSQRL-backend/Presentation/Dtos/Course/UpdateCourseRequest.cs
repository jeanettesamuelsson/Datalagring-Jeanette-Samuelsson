namespace Presentation.Dtos.Course;

public sealed record UpdateCourseRequest( 

    Guid Id,
    string CourseName,
    string CourseCode,
    string Description,
    byte[] RowVersion

);
