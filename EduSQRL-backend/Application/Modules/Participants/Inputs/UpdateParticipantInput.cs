using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Modules.Participants.Inputs;

public record UpdateParticipantInput
(
    Guid Id,
    string FirstName,
    string LastName,
    string Email
);
