

using Application.Abstractions.Persistence;
using Application.Modules.Participants.Inputs;
using Application.Modules.Participants.Outputs;
using Application.Modules.Participants.PersistanceModels;
using Domain.Participants.ValueObjects;
using System.Reflection.Metadata;

namespace Application.Modules.Participants;

public class ParticipantService
    (
    IParticipantRepository participants,
    IUnitOfWork uow

    ): IParticipantService
{
    private static ParticipantOutput ToOutputModel(Participant p) => new(
        p.Id,
        p.Email,
        p.Created,
        p.RowVersion

        );

    // create 
    public async Task<Guid> CreateAsync(CreateParticipantInput input, CancellationToken ct)
    {
        var email = new Email(input.Email);
        var phoneNumber = string.IsNullOrWhiteSpace(input.PhoneNumber)
            ? null : new PhoneNumber(input.PhoneNumber);

        if (await participants.EmailAlreadyExistsAsync(email.Value, ct))
           throw new ArgumentException("Email already exists");

        var participantId = Guid.NewGuid();
        var dateNow = DateTime.UtcNow;

        var participant = new Participant(
            Id: participantId,
            Email: email.Value,
            Created: dateNow,
            RowVersion: Array.Empty<byte>()
            );

        await participants.AddAsync(participant);

        await uow.SaveChangesAsync(ct);

        return participantId;

    }

    // read (all and by id)
    public async Task<IReadOnlyList<ParticipantOutput>> GetAllParticipantsAsync(CancellationToken ct = default)
    {
       var list = await participants.ListAsync(ct);

        return [.. list.Select(ToOutputModel)];
    }

    public async Task<ParticipantOutput?> GetByIdAsync(Guid participantId, CancellationToken ct)
    {
        var participant = await participants.GetByIdAsync(participantId, ct);

        return participant is null ? null : ToOutputModel(participant);
    }


    //update
    public async Task<ParticipantOutput?> UpdateAsync(UpdateParticipantInput input, CancellationToken ct)
    {
        
        var participant = await participants.GetByIdAsync(input.Id, ct);
        if (participant is null)
            return null;

        var updatedParticipant = participant with
        {
            Email = input.Email,
            RowVersion = input.RowVersion
        };

        
        await participants.UpdateAsync(updatedParticipant, ct);

        await uow.SaveChangesAsync(ct);

        return ToOutputModel(updatedParticipant);
    }

    //delete
    public async Task DeleteAsync(Guid participantId, byte[] rowVersion, CancellationToken ct = default)
    {
        var participant = await participants.GetByIdAsync(participantId, ct)
                       ?? throw new ArgumentException("Participant not found");

        await participants.UpdateAsync(participant with { RowVersion = rowVersion }, ct);

        await participants.DeleteAsync(participantId, ct);

        await uow.SaveChangesAsync(ct);
    }


}


