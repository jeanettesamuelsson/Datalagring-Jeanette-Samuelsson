using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Modules.Participants.Outputs;

public record ParticipantOutput
(
    Guid Id, 
    string Email,
    DateTime CreatedAt,
    byte[] RowVersion


);
