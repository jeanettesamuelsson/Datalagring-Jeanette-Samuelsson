namespace Presentation.Dtos;

public record ParticipantDto(

    Guid Id,
    string FirstName,
    string LastName,
    string Email,
    string PhoneNumber

);