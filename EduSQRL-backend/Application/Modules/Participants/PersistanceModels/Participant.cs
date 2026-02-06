
namespace Application.Modules.Participants.PersistanceModels;

public sealed record Participant(
    Guid Id, 
    string Email, 
    DateTime Created, 
    byte [] RowVersion

);

