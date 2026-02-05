

using Application.Modules.Participants.Inputs;
using Application.Modules.Participants.Outputs;

namespace Application.Modules.Participants;

public class ParticipantService : IParticipantService
{

    private static List<ParticipantOutput> _participants = [];

   
    // create 
    public async Task<ParticipantOutput> CreateAsync(CreateParticipantInput input, CancellationToken cancellationToken)
    {
        var participant = new ParticipantOutput(Guid.NewGuid(), input.FirstName, input.LastName, input.Email);
        _participants.Add(participant);

        return await Task.FromResult(participant);
    }

    // read (all and by id)
    public async Task<IEnumerable<ParticipantOutput>> GetAllParticipantsAsync(CancellationToken cancellationToken)
    {
        return await Task.FromResult(_participants);
    }

    public async Task<ParticipantOutput?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var participant = _participants.FirstOrDefault(p => p.Id == id);
        return await Task.FromResult(participant);
    }


    //update
    public async Task<ParticipantOutput?> UpdateAsync(UpdateParticipantInput input, CancellationToken cancellationToken)
    {
        var index = _participants.FindIndex(p => p.Id == input.Id);
        if (index == -1)
        {
            return null;
        }

        var existingParticipant = _participants[index];
        var updatedParticipant = existingParticipant with
        {
            FirstName = input.FirstName,
            LastName = input.LastName,
            Email = input.Email
        };

        _participants[index] = updatedParticipant;

        return await Task.FromResult(updatedParticipant);
    }

    //delete
    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var index = _participants.FindIndex(p => p.Id == id);
        if (index == -1)
        {
            return false;
        }

        _participants.RemoveAt(index);

        return await Task.FromResult(true);
    }
}


