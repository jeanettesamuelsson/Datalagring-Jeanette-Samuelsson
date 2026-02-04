using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Modules.Participants.Outputs;

public record ParticipantOutput
(
    Guid Id, 
    string FirstName,
    string LastName,
    string Email
 
);
