using Domain.Participants.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Modules.Participants.Inputs;

public record UpdateParticipantInput
(
    Guid Id,
    string FirstName,
    string LastName,
    string Email,
    string PhoneNumber,
    List<string> Roles,
    byte[] RowVersion
);
