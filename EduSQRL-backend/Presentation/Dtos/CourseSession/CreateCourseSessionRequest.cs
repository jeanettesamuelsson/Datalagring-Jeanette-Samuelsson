namespace Presentation.Dtos.CourseSession;

public sealed record CreateCourseSessionRequest(

    Guid CourseId,
    Guid LocationId,
    string CourseName, 
    string LocationName,
    DateTime StartDate,
    DateTime EndDate,
    int Capacity

);
