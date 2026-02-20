

using Domain.Models;
using Domain.Persistence;

namespace Domain.RepositoryInterfaces;

public interface IParticipantRepository : IBaseRepository<Participant, Guid>
{
    //method to check that emails does not already exist when creating a participant
    Task<bool> EmailAlreadyExistsAsync(string email, CancellationToken ct = default);


}

