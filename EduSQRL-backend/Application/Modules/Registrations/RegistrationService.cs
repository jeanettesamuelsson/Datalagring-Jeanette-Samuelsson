using Application.Abstractions.Persistence;
using Application.Modules.Registrations.Input;
using Application.Modules.Registrations.Output;

using Domain.Models;

namespace Application.Modules.Registrations;

public class RegistrationService
    (
    IRegistrationRepository registrations,
    IUnitOfWork uow

    ) : IRegistrationService

{

    private static RegistrationOutput ToOutputModel(Registration r) => new(

        r.Id,
        r.ParticipantId,
        r.CourseSessionId,
        r.ParticipantName,
        r.CourseName,
        r.Created,
        r.Status.ToString(),
        r.RowVersion

        );

    // create 
    public async Task<Guid> CreateAsync(CreateRegistrationInput input, CancellationToken ct)
    {

        if (await registrations.AlreadyExistsAsync(input.ParticipantId, input.CourseSessionId, ct))
            throw new ArgumentException("Already Registered!");


        var registrationId = Guid.NewGuid();

        var registration = new Registration(

            Id: registrationId,
            ParticipantId: input.ParticipantId,
            ParticipantName: "",
            CourseSessionId: input.CourseSessionId,
            CourseName: "",
            Status: RegistrationStatus.Pending,
            Created: DateTime.UtcNow,
            RowVersion: []

            );

        await registrations.AddAsync(registration);

        await uow.SaveChangesAsync(ct);

        return registrationId;

    }

    // read (all and by id)
    public async Task<IReadOnlyList<RegistrationOutput>> GetAllAsync(CancellationToken ct = default)
    {
        var list = await registrations.ListAsync(ct);

        return [.. list.Select(ToOutputModel)];
    }

    public async Task<RegistrationOutput?> GetByIdAsync(Guid registrationId, CancellationToken ct)
    {
        var registration = await registrations.GetByIdAsync(registrationId, ct);

        return registration is null ? null : ToOutputModel(registration);
    }


    //update
    public async Task<RegistrationOutput?> UpdateAsync(UpdateRegistrationInput input, CancellationToken ct)
    {

        var registration = await registrations.GetByIdAsync(input.Id, ct);
        if (registration is null)
            return null;


        var updatedRegistration = registration with
        {
            Status = input.Status,
            RowVersion = input.RowVersion

        };

        // update and save changes

        await registrations.UpdateAsync(updatedRegistration, ct);
        await uow.SaveChangesAsync(ct);

        // return the updated registration

        var updated = await registrations.GetByIdAsync(input.Id, ct);
        return updated is null ? null : ToOutputModel(updated);


    }

    //delete
    public async Task DeleteAsync(Guid registrationId, byte[] rowVersion, CancellationToken ct = default)
    {
        var registration = await registrations.GetByIdAsync(registrationId, ct)
                       ?? throw new ArgumentException("Registration not found");

        await registrations.UpdateAsync(registration with { RowVersion = rowVersion }, ct);

        await registrations.DeleteAsync(registrationId, ct);

        await uow.SaveChangesAsync(ct);
    }

    
}

