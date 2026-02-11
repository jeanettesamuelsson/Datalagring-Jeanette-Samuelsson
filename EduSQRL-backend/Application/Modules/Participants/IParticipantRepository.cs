
using Application.Abstractions.Persistence;
using Application.Modules.PersistanceModels;


namespace Application.Modules.Participants;

public interface IParticipantRepository : IBaseRepository<Participant, Guid>
{
    //method to check that emails does not already exist
    Task<bool> EmailAlreadyExistsAsync(string email, CancellationToken ct = default);


}

