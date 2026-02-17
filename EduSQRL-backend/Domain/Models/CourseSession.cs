
using System.Xml.Linq;

namespace Domain.Models;

public sealed record CourseSession(

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

