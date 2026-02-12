

namespace Domain.Models;

public sealed record Course
(
    Guid Id,
    string CourseName,
    string CourseCode,
    string Description,
    byte[] RowVersion


    );
