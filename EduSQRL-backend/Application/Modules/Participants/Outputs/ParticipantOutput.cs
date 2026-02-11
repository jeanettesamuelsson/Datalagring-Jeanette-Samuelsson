
namespace Application.Modules.Participants.Outputs;

public record ParticipantOutput
(
    Guid Id, 
    string FirstName,
    string LastName,
    string Email,
    string PhoneNumber,
    List<string> Roles,
    DateTime Created,
    byte[] RowVersion


);
