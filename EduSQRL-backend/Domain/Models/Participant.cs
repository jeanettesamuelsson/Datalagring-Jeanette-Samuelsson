using Domain.Participants.ValueObjects;

namespace Domain.Models;

public sealed record Participant(
    Guid Id, 
    string FirstName,
    string LastName,
    string Email,
    PhoneNumber PhoneNumber,
    Guid RoleId,
    string RoleName,
    DateTime Created, 
    byte [] RowVersion
    

);

