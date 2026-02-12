namespace Presentation.Dtos.Course;

public sealed record CourseDto(

    Guid Id,
    string CourseCode,
    string CourseName,
    string Description,
    byte[] RowVersion

);
