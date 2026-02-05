namespace Presentation.Dtos;

public record UpdateParticipantRequest
(
    Guid Id,
    string FirstName,
    string LastName,
    string Email
);
