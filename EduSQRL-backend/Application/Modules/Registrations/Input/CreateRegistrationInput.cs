using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Modules.Registrations.Input;

public sealed record CreateRegistrationInput(

    Guid ParticipantId,
    Guid CourseSessionId

);
