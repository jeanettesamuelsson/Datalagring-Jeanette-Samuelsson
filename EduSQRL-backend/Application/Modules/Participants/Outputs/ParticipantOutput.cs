
namespace Application.Modules.Participants.Outputs;

public record ParticipantOutput
(
    Guid Id, 
    string FirstName,
    string LastName,
    string Email,
    string PhoneNumber,
    Guid RoleId,
    string RoleName,
    DateTime Created,
    byte[] RowVersion


);
