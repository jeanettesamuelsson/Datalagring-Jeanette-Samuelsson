

namespace Application.Modules.CourseSessions.Output;

public sealed record CourseSessionOutput(

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
