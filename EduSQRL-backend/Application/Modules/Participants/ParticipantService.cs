

using Application.Abstractions.Persistence;
using Application.Modules.Participants.Inputs;
using Application.Modules.Participants.Outputs;
using Domain.Participants.ValueObjects;
using Application.Modules.Roles;
using Domain.Models;


namespace Application.Modules.Participants;

public class ParticipantService

    // primary constructor
    (
    IParticipantRepository participants,
    IUnitOfWork uow,
    IRoleService roleService

    ) : IParticipantService


{

    private static ParticipantOutput ToOutputModel(Participant p) => new(
        p.Id,
        p.FirstName,
        p.LastName,
        p.Email,
        p.PhoneNumber.Value,
        p.RoleId,
        p.RoleName,
        p.Created,
        p.RowVersion

        );

    // create 
    public async Task<Guid> CreateAsync(CreateParticipantInput input, CancellationToken ct)
    {
        var email = new Email(input.Email);
        var phoneNumber = new PhoneNumber(input.PhoneNumber);

        // call repo method to check if email is already in database => true, throw exception

        if (await participants.EmailAlreadyExistsAsync(email.Value, ct))
           throw new ArgumentException("Email already exists");

        //validate roles input

        var roles = await roleService.GetRolesAsync(ct);

        if (!roles.Any(r => r.Id == input.RoleId))
            throw new ArgumentException($"Invalid RoleId: {input.RoleId}");



        var participantId = Guid.NewGuid();
        var dateNow = DateTime.UtcNow;

        var participant = new Participant(
            Id: participantId,
            FirstName: input.FirstName,
            LastName: input.LastName,
            Email: email.Value,
            PhoneNumber: phoneNumber,
            RoleId: input.RoleId,
            RoleName: "",
            Created: dateNow,
            RowVersion: []

            );

        // call repository method AddAsync

        await participants.AddAsync(participant);

        // save changes in unit of work

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
        // call repo

        var participant = await participants.GetByIdAsync(input.Id, ct);
        if (participant is null)
            return null;

        //validate roles input

        var roles = await roleService.GetRolesAsync(ct);

        if (!roles.Any(r => r.Id == input.RoleId))
            throw new ArgumentException($"Invalid RoleId: {input.RoleId}");

        var phoneNumber = new PhoneNumber(input.PhoneNumber);

        var updatedParticipant = participant with
        {
            FirstName = input.FirstName,
            LastName = input.LastName,
            Email = input.Email,
            PhoneNumber = phoneNumber,
            RoleId = input.RoleId,
            RowVersion = input.RowVersion
        };

        // update and save changes

        await participants.UpdateAsync(updatedParticipant, ct);
        await uow.SaveChangesAsync(ct);

        // return the updated participant

        var updated = await participants.GetByIdAsync(input.Id, ct);
        return updated is null ? null : ToOutputModel(updated);

       
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


