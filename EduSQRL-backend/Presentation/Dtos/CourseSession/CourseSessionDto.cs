namespace Presentation.Dtos.CourseSession;

public sealed record CourseSessionDto(

    Guid Id,
    Guid CourseId,
    Guid LocationId,
    string CourseName,
    string LocationName,
    DateTime StartDate,
    DateTime EndDate,
    int Capacity,
    byte[] RowVersion

);
