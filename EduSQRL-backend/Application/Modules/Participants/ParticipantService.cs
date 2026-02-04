

using Application.Modules.Participants.Inputs;
using Application.Modules.Participants.Outputs;

namespace Application.Modules.Participants;

public class ParticipantService
{

    private static List<ParticipantOutput> _participants = [];

    public ParticipantOutput Create(CreateParticipantInput input)
    {
        var participant = new ParticipantOutput(Guid.NewGuid(), input.FirstName, input.LastName, input.Email);
        _participants.Add(participant);

        return participant;
    }

    public IEnumerable<ParticipantOutput> GetAllParticipants()
    {
        return _participants;
    }

    public ParticipantOutput? Update(UpdateParticipantInput input)
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

        return updatedParticipant;
    }

    public void Delete (Guid id)
    {
       var index = _participants.FindIndex(p => p.Id == id);
       if (index == -1)
       {
            return;
       }

        _participants.RemoveAt(index);
    }


}
