
namespace Domain.Models;

public sealed record CourseSession(

    Guid Id,
    Guid CourseId,
    Guid LocationId,
    DateTime StartDate,
    DateTime EndDate,
    int Capacity,
    byte[] RowVersion
    
);

