using Application.Abstractions.Persistence;
using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Persistence.Entities;

public class RoleEntity : IEntity<Guid>
{
    public Guid Id { get; set; }
    public string RoleName { get; set; } = null!;
    public byte[] Concurrency { get; set; } = null!;
    public DateTime Created { get; set; }
    public DateTime Modified { get; set; }

    // navgation property

    public ICollection<ParticipantEntity> Participants { get; set; } = [];

}

