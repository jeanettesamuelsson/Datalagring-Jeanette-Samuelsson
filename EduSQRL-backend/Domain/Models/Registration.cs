using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Models;

public sealed record Registration(

    Guid Id,
    Guid ParticipantId,
    string ParticipantName, 
    Guid CourseSessionId,
    string CourseName,      
    RegistrationStatus Status,
    DateTime Created,
    byte[] RowVersion

);
