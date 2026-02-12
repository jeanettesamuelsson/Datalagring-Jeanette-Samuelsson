namespace Presentation.Dtos.Course;

public sealed record UpdateCourseRequest( 

    Guid Id,
    string CourseCode,
    string CourseName,
    string Description,
    byte[] RowVersion

);
