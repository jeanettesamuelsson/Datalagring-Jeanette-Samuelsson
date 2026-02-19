namespace Presentation.Dtos.Course;

public sealed record CreateCourseRequest(

    string CourseName,
    string CourseCode,
    string Description

);
