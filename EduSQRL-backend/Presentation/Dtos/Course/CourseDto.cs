namespace Presentation.Dtos.Course;

public sealed record CourseDto(

    Guid Id,
    string CourseName,
    string CourseCode,
    string Description,
    byte[] RowVersion

);
