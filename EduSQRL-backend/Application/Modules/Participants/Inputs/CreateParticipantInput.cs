using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Modules.Participants.Inputs;

public record CreateParticipantInput
(
    string FirstName,
    string LastName,
    string Email
   
);
