using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Modules.CourseSessions.Input;

public sealed record UpdateCourseSessionInput(

    Guid Id,
    Guid CourseId,
    Guid LocationId,
    DateTime StartDate,
    DateTime EndDate,
    int Capacity,
    byte[] RowVersion

);
