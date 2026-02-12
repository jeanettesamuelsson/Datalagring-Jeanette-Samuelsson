using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Modules.CourseSessions.Input;

public sealed record CreateCourseSessionInput( 

    Guid CourseId,
    Guid LocationId,
    DateTime StartDate,
    DateTime EndDate,
    int Capacity

);
