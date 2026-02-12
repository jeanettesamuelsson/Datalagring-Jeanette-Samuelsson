using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Modules.Courses.Input;

public record UpdateCourseInput
(
    Guid Id,
    string CourseName,
    string CourseCode,
    string Description,
    byte[] RowVersion

    );


