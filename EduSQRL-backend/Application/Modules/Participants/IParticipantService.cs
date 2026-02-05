using Application.Modules.Participants.Inputs;
using Application.Modules.Participants.Outputs;

namespace Application.Modules.Participants;

public interface IParticipantService
{
    // create
    Task<ParticipantOutput> CreateAsync(CreateParticipantInput input, CancellationToken cancellationToken);
    
    // delete
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken);
    
    // get all 
    Task<IEnumerable<ParticipantOutput>> GetAllParticipantsAsync(CancellationToken cancellationToken);
    
    // get by ID
    Task<ParticipantOutput?> GetByIdAsync(Guid id, CancellationToken cancellationToken);

    // update
    Task<ParticipantOutput?> UpdateAsync(UpdateParticipantInput input, CancellationToken cancellationToken);
}