using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Modules.Courses.Output;

public record CourseOutput
(
    Guid Id,
    string CourseName,
    string CourseCode,
    string Description,
    byte[] RowVersion

    );
