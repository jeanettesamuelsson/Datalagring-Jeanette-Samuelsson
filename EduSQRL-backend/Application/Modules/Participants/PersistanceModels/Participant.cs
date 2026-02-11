
using Domain.Participants.ValueObjects;

namespace Application.Modules.Participants.PersistanceModels;

public sealed record Participant(
    Guid Id, 
    string FirstName,
    string LastName,
    string Email,
    PhoneNumber PhoneNumber,
    List<string> Roles,
    DateTime Created, 
    byte [] RowVersion
    

);

