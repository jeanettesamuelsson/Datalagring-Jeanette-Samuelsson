using Application.Modules.Participants.Inputs;
using Application.Modules.Participants.Outputs;

namespace Application.Modules.Participants;

public interface IParticipantService
{
    // create
    Task<Guid> CreateAsync(CreateParticipantInput input, CancellationToken cancellationToken);
    
    // delete
    Task DeleteAsync(Guid participantId, byte[] rowVersion, CancellationToken cancellationToken);
    
    // get all 
    Task<IReadOnlyList<ParticipantOutput>> GetAllParticipantsAsync(CancellationToken cancellationToken);
    
    // get by ID
    Task<ParticipantOutput?> GetByIdAsync(Guid id, CancellationToken cancellationToken);

    // update
    Task<ParticipantOutput?> UpdateAsync(UpdateParticipantInput input, CancellationToken cancellationToken);
}