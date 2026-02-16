using Application.Modules.Participants.Inputs;
using Application.Modules.Participants.Outputs;
using Application.Modules.Registrations.Input;
using Application.Modules.Registrations.Output;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Modules.Registrations;

public interface IRegistrationService
{

    // create
    Task<Guid> CreateAsync(CreateRegistrationInput input, CancellationToken cancellationToken);

    // delete
    Task DeleteAsync(Guid registrationId, byte[] rowVersion, CancellationToken cancellationToken);

    // get all 
    Task<IReadOnlyList<RegistrationOutput>> GetAllAsync(CancellationToken cancellationToken);

    // get by ID
    Task<RegistrationOutput?> GetByIdAsync(Guid id, CancellationToken cancellationToken);

    // update
    Task<RegistrationOutput?> UpdateAsync(UpdateRegistrationInput input, CancellationToken cancellationToken);
}
