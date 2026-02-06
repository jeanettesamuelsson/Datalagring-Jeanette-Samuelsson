using Application.Abstractions.Persistence;

namespace Infrastructure.Persistence.Entities;

public class ParticipantEntity : IEntity<Guid>
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string? PhoneNumber { get; set; } 
    public byte[] Concurrency { get; set; } = null!;
    public DateTime Created { get; set; }
    public DateTime Modified { get; set; }
    public ICollection<RoleEntity> Roles { get; set; } = [];




}
   
