using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Modules.Courses.Input;

public record CreateCourseInput
(
    string CourseName,
    string CourseCode,
    string Description
);
