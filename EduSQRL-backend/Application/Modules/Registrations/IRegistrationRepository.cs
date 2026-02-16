using Application.Abstractions.Persistence;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Modules.Registrations;

public interface IRegistrationRepository : IBaseRepository<Registration, Guid>
{
    //method to check that a participant is not already registered to a session
    Task<bool> AlreadyExistsAsync(Guid participantId, Guid courseSessionId, CancellationToken ct = default);


}
