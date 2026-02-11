
namespace Application.Modules.Participants.Inputs;

public record CreateParticipantInput
(
    string FirstName,
    string LastName,
    string Email,
    string PhoneNumber,
    List<string> Roles

);
