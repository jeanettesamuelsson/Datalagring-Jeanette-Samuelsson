namespace Presentation.Dtos.CourseSession;

public sealed record CreateCourseSessionRequest(

    Guid CourseId,
    Guid LocationId,
    DateTime StartDate,
    DateTime EndDate,
    int Capacity

);
