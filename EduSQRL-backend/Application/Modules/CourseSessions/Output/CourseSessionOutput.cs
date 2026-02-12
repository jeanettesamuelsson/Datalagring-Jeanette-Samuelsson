using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Modules.CourseSessions.Output;

public sealed record CourseSessionOutput(

    Guid Id,
    Guid CourseId,
    Guid LocationId,
    DateTime StartDate,
    DateTime EndDate,
    int Capacity,
    byte[] RowVersion

);
