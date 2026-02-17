using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Modules.CourseSessions.Input;

public sealed record CreateCourseSessionInput( 

    Guid CourseId,
    Guid LocationId,
    string CourseName,
    string LocationName, 
    DateTime StartDate,
    DateTime EndDate,
    int Capacity

);
