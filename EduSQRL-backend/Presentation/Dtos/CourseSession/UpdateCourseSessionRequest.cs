namespace Presentation.Dtos.CourseSession;

public sealed record UpdateCourseSessionRequest(

    Guid Id,
    Guid CourseId,
    Guid LocationId,
    DateTime StartDate,
    DateTime EndDate,
    int Capacity,
    byte[] RowVersion 

);
