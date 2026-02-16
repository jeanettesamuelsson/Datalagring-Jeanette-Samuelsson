using Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Modules.Registrations.Output;

public sealed record RegistrationOutput(

    Guid Id,
    Guid ParticipantId,
    Guid CourseSessionId,
    string ParticipantName, 
    string CourseName,     
    DateTime Created,
    string Status,
    byte[] RowVersion



);
