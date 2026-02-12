namespace Presentation.Dtos.Course;

public sealed record CreateCourseRequest(

    string CourseCode,
    string CourseName,
    string Description

);
