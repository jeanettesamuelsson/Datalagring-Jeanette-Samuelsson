using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Persistence.Entities;

public class RoleEntity
{
    public int Id { get; set; }
    public string RoleName { get; set; } = null!;
    public ICollection<ParticipantEntity> Participants { get; set; } = [];
    public byte[] Concurrency { get; set; } = null!;
    public DateTime Created { get; set; }
    public DateTime Modified { get; set; }

}

