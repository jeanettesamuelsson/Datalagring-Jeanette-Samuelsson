
using Domain.Models;
using Domain.Persistence;

namespace Domain.RepositoryInterfaces;

public interface IRegistrationRepository : IBaseRepository<Registration, Guid>
{
    //method to check that a participant is not already registered to a session
    Task<bool> AlreadyExistsAsync(Guid participantId, Guid courseSessionId, CancellationToken ct = default);


}
