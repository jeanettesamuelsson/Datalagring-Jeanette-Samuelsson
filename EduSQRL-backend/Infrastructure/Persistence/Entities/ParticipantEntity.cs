using Application.Abstractions.Persistence;

namespace Infrastructure.Persistence.Entities;

public class ParticipantEntity : IEntity<Guid>
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string PhoneNumber { get; set; } = null!;
    public byte[] Concurrency { get; set; } = null!;
    public DateTime Created { get; set; }
    public DateTime Modified { get; set; }
    public Guid RoleId { get; set; } //FK

    // navigation properties
    public RoleEntity Role { get; set; } = null!; 

    public ICollection<RegistrationEntity> Registrations { get; set; } = [];


}
   
